﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Controller : MonoBehaviour {

	[Tooltip("_sceneToLoadOnPlay is the name of the scene that will be loaded when users click play")]
	public string _sceneToLoadOnPlay;
	[Tooltip("_webpageURL defines the URL that will be opened when users click on your branding icon")]
	[HideInInspector] public string _webpageURL = "http://www.alpaca.studio";
	[Tooltip("_soundButtons define the SoundOn[0] and SoundOff[1] Button objects.")]
	public Button[] _soundButtons;
	[Tooltip("_audioClip defines the audio to be played on button click.")]
	public AudioClip _audioClip;
	[Tooltip("_audioSource defines the Audio Source component in this scene.")]
	public AudioSource _audioSource;
	
	//The private variable 'scene' defined below is used for example/development purposes.
	//It is used in correlation with the Escape_Menu script to return to last scene on key press.
	UnityEngine.SceneManagement.Scene scene;

	void Awake () {
		if(!PlayerPrefs.HasKey("_Mute")){
			PlayerPrefs.SetInt("_Mute", 0);
		}
		
		scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
		PlayerPrefs.SetString("_LastScene", scene.name.ToString()); 
		//Debug.Log(scene.name);
	}
	
	public void OpenWebpage () {
		_audioSource.PlayOneShot(_audioClip);
		Application.OpenURL(_webpageURL);
	}
	
	public void StartGame ()
	{
		// StartCoroutine("WaitForAudioClip");
		StartCoroutine(GameManager.Instance.PrepStartOfGame(gameObject.transform.parent.gameObject));
		// PlayerPrefs.SetString("_LastScene", scene.name);
		// UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneToLoadOnPlay);
	}

	public void RestartGame() 
	{
		StartCoroutine(SoundManager.Instance.PlayStartButtonClip());

		ResetInstances();
		SceneManager.LoadScene(scene.name);
		// SceneManager.LoadScene(scene.ToString());
	}


	// Improve later.
	void ResetInstances()
	{
		EnemySpawner.Instance.ResetSingleton();
		FireMissile.Instance.ResetSingleton();
		PlayerController.Instance.ResetSingleton();
		SoundManager.Instance.ResetSingleton();
		GameManager.Instance.ResetSingleton();
	}

	// IEnumerator WaitForAudioClip()
	// {
		
	// 	audioSource.clip.length;
	// 	// _audioSource.PlayOneShot(_audioClip);

	// 	Debug.Log(clipLength);
	// 	Debug.Log("waiting...");
	// 	yield return new WaitUntil(() => !audioSource.isPlaying);
	// 	Debug.Log("waiting ended.");
	// }


	
	public void Mute () {
		_audioSource.PlayOneShot(_audioClip);
		_soundButtons[0].interactable = true;
		_soundButtons[1].interactable = false;
		PlayerPrefs.SetInt("_Mute", 1);
	}
	
	public void Unmute () {
		_audioSource.PlayOneShot(_audioClip);
		_soundButtons[0].interactable = false;
		_soundButtons[1].interactable = true;
		PlayerPrefs.SetInt("_Mute", 0);
	}

	public void ShowLeaderboard()
	{
		// do nothing (for now?)
	}
	
	// public void QuitGame () {
	// 	_audioSource.PlayOneShot(_audioClip);
	// 	#if !UNITY_EDITOR
	// 		Application.Quit();
	// 	#endif
		
	// 	#if UNITY_EDITOR
	// 		UnityEditor.EditorApplication.isPlaying = false;
	// 	#endif
	// }
}