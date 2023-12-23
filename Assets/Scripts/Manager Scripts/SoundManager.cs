using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [HideInInspector] AudioSource audioSource;

    public AudioClip startButtonClip;
    public AudioClip enemyExplosionClip;

    float volume;

    void Start()
    {
        Instance = this;

        // DontDestroyOnLoad(this.gameObject);

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator PlayStartButtonClip()
    {
        audioSource.volume = 0.5f;
        audioSource.clip = startButtonClip;
        yield return StartCoroutine(PlayClip(audioSource.clip));
    }

    public IEnumerator PlayExplosionClip()
    {
        audioSource.volume = 0.2f;
        audioSource.clip = enemyExplosionClip;
        yield return StartCoroutine(PlayClip(audioSource.clip));
    }


    IEnumerator PlayClip(AudioClip clip)
    {
        audioSource.Play();
        yield return new WaitUntil(() => !audioSource.isPlaying);
    }
}
