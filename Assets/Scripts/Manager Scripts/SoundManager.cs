using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [HideInInspector] AudioSource audioSource;

    [SerializeField] AudioClip menuAmbienceClip;
    [SerializeField] AudioClip startButtonClip;
    [SerializeField] AudioClip enemyExplosionClip;

    float volume;

    void Start()
    {
        Instance = this;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        

        StartCoroutine(PlayClip(menuAmbienceClip, 0.15f, false));
    }


    public IEnumerator FadeVolumeOverTime(float fadeTime)
    {
        // float currentVolume = audioSource.volume;
        int fadeIncrementations = 40;

        for (int i = 0; i < fadeIncrementations; i++)
        {
            audioSource.volume -= (audioSource.volume / fadeIncrementations);
            yield return new WaitForSeconds(fadeTime / fadeIncrementations);
        }

        yield break;
    }
    
    public IEnumerator PlayStartButtonClip()
    {        
        yield return StartCoroutine(PlayClip(startButtonClip, 0.15f, true));
        // yield return StartCoroutine(SoundManager.Instance.FadeVolumeOverTime(1f));
    }

    // public IEnumerator RestartingGame()
    // {
    //     AudioSource gameplayAmbience = GameManager.Instance.GetComponent<AudioSource>();        
    //     yield return StartCoroutine(PlayClip(gameplayAmbience.clip, gameplayAmbience.volume, false));

    //     yield return StartCoroutine(FadeVolumeOverTime(0.2f));
    // }

    public IEnumerator PlayStartButtonThenStartGame()
    {
                            // (will by default use menuAmbienceClip)
        yield return StartCoroutine(PlayClip(menuAmbienceClip, 0.15f, false));
        yield return StartCoroutine(PlayClip(startButtonClip, 0.15f, false));
        yield return new WaitForSeconds(0.2f); // shorter than button's clip length
    }
    
    public IEnumerator PlayExplosionClip()
    {
        yield return StartCoroutine(PlayClip(enemyExplosionClip, 0.13f, true));
    }


    IEnumerator PlayClip(AudioClip newClip, float newVolume, bool preventMultiplePlays)
    {
        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();
        audioSource.volume = newVolume;

        if (preventMultiplePlays)
            yield return new WaitUntil(() => !audioSource.isPlaying);
        yield break;
    }

    public void ResetSingleton() => Instance = null;
}
