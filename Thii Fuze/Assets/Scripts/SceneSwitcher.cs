﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour 
{
	Button back;

	Button startGame;
	Button highScore;
	Button credits;
	Button quit;

	Button win;
	Button lose;

	Scene[] scenes;

	void Awake()
	{
		SceneManager.LoadScene(1, LoadSceneMode.Additive);
		scenes = SceneManager.GetAllScenes();
	}

	void Start()
	{
		startGame = GameObject.Find("Play").GetComponent<Button>();
		highScore = GameObject.Find("High Score").GetComponent<Button>();
		credits = GameObject.Find("Credits").GetComponent<Button>();
		quit = GameObject.Find("Quit").GetComponent<Button>();

		startGame.onClick.AddListener (delegate {
			StartCoroutine(StartGame());
		});
		highScore.onClick.AddListener (delegate {
			StartCoroutine(SeeHighScore());
		});
		credits.onClick.AddListener (delegate {
			StartCoroutine(SeeCredits());
		});
		quit.onClick.AddListener (Quit);
	}

	IEnumerator Back()
	{
		SceneManager.UnloadSceneAsync(scenes[1]);
		AsyncOperation loading = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
		yield return new WaitUntil(() => loading.isDone);
		startGame = GameObject.Find("Play").GetComponent<Button>();
		highScore = GameObject.Find("High Score").GetComponent<Button>();
		credits = GameObject.Find("Credits").GetComponent<Button>();
		quit = GameObject.Find("Quit").GetComponent<Button>();

		startGame.onClick.AddListener (delegate {
			StartCoroutine(StartGame());
		});
		highScore.onClick.AddListener (delegate {
			StartCoroutine(SeeHighScore());
		});
		credits.onClick.AddListener (delegate {
			StartCoroutine(SeeCredits());
		});
		quit.onClick.AddListener (Quit);
		scenes = SceneManager.GetAllScenes();
	}

	IEnumerator StartGame()
	{
		SceneManager.UnloadSceneAsync(scenes[1]);
		AsyncOperation loading = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
		yield return new WaitUntil(() => loading.isDone);
//		win = GameObject.Find("Win").GetComponent<Button>();
//		lose = GameObject.Find("Lose").GetComponent<Button>();
//		win.onClick.AddListener (delegate {
//			StartCoroutine(Win());
//		});
//		lose.onClick.AddListener (delegate {
//			StartCoroutine(Lose());
//		});
		scenes = SceneManager.GetAllScenes();
	}

	IEnumerator Win()
	{
		SceneManager.UnloadSceneAsync(scenes[1]);
		AsyncOperation loading = SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
		yield return new WaitUntil(() => loading.isDone);
		back = GameObject.Find("Back").GetComponent<Button>();
		back.onClick.AddListener (delegate {
			StartCoroutine(Back());
		});
		scenes = SceneManager.GetAllScenes();
	}

	IEnumerator Lose()
	{
		SceneManager.UnloadSceneAsync(scenes[1]);
		AsyncOperation loading = SceneManager.LoadSceneAsync(4, LoadSceneMode.Additive);
		yield return new WaitUntil(() => loading.isDone);
		back = GameObject.Find("Back").GetComponent<Button>();
		back.onClick.AddListener (delegate {
			StartCoroutine(Back());
		});
		scenes = SceneManager.GetAllScenes();
	}

	IEnumerator SeeHighScore()
	{
		SceneManager.UnloadSceneAsync(scenes[1]);
		AsyncOperation loading = SceneManager.LoadSceneAsync(5, LoadSceneMode.Additive);
		yield return new WaitUntil(() => loading.isDone);
		back = GameObject.Find("Back").GetComponent<Button>();
		back.onClick.AddListener (delegate {
			StartCoroutine(Back());
		});
		scenes = SceneManager.GetAllScenes();
	}

	IEnumerator SeeCredits()
	{
		SceneManager.UnloadSceneAsync(scenes[1]);
		AsyncOperation loading = SceneManager.LoadSceneAsync(6, LoadSceneMode.Additive);
		yield return new WaitUntil(() => loading.isDone);
		back = GameObject.Find("Back").GetComponent<Button>();
		back.onClick.AddListener (delegate {
			StartCoroutine(Back());
		});
		scenes = SceneManager.GetAllScenes();
	}

	void Quit()
	{
		Debug.Log("Quit");
		Application.Quit();
	}
}