using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour {

	public Button startButton;


	private void Start()
	{
		startButton.onClick.AddListener (OnStartButton);
	}

	private void OnDestroy()
	{
		startButton.onClick.RemoveListener (OnStartButton);
	}

	private void OnStartButton()
	{
		SceneManager.LoadScene ("Main");
	}
}
