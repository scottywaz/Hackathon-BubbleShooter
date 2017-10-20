/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

	public Text _centerText;
	public Text _score;
	public Text _timeRemaining;
	public Text _objectiveProgress;

	public GameObject resultsWindow;
	public GameObject hud;

	public Button continueButton;
	public Text resultsScore;
	public Text resultsObjective;
	public Text timeBonus;

	private int _currentScore = 0;
	private int _currentObjective = 0;
	private float _timeLeft = 0;

	bool isGameOver = false;

	// Use this for initialization
	void Start ()
	{
		continueButton.onClick.AddListener (OnContinue);
	}

	private void OnDestroy()
	{
		continueButton.onClick.RemoveListener (OnContinue);
	}

	public void DisplayGameOver ()
	{
		_centerText.transform.parent.gameObject.SetActive (true);
		_centerText.text = "Oh fudge!\nGame Over";
		StartCoroutine (ShowResults (false));
		isGameOver = true;
	}

	public void DisplayWin ()
	{
		_centerText.transform.parent.gameObject.SetActive (true);
		_centerText.text = "Win!";
		StartCoroutine (ShowResults (true));
		isGameOver = true;
	}

	public void UpdateScore (int score)
	{
		_score.text = score.ToString ();
		_currentScore = score;
	}

	public void UpdateTimeRemaining (float time)
	{
		if (isGameOver)
			return;
		
		_timeLeft = time;
		var minutes = time / 60; //Divide the guiTime by sixty to get the minutes.
		var seconds = time % 60;//Use the euclidean division for the seconds.

		_timeRemaining.text = string.Format ("{0:0}:{1:00}", minutes, seconds);
	}

	public void UpdateObjectiveProgress (int objective)
	{
		_currentObjective = objective;
		_objectiveProgress.text = objective.ToString ();
	}

	public void DisableText ()
	{
		_centerText.transform.parent.gameObject.SetActive (false);
	}

	public IEnumerator ShowResults(bool playerWon)
	{
		yield return new WaitForSeconds (2f);
		resultsWindow.SetActive (true);
		hud.SetActive (false);
		_centerText.transform.parent.gameObject.SetActive (false);


		resultsObjective.text = _currentObjective.ToString();


		if (playerWon) 
		{
			int bonus = (int)(50f * _timeLeft);
			//needs time bonus calculation
			timeBonus.text = bonus.ToString();
			int totalScore = _currentScore + bonus;

			resultsScore.text = totalScore.ToString();
		} 
		else
		{
			timeBonus.text = "0";
			resultsScore.text = _currentScore.ToString ();
		}
	}

	void OnContinue()
	{
		SceneManager.LoadScene ("Start");
	}

}
