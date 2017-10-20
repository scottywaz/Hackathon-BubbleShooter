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
		StartCoroutine (ShowResults ());
	}

	public void DisplayWin ()
	{
		_centerText.transform.parent.gameObject.SetActive (true);
		_centerText.text = "Win!";
		StartCoroutine (ShowResults ());
	}

	public void UpdateScore (int score)
	{
		_score.text = score.ToString ();
		_currentScore = score;
	}

	public void UpdateTimeRemaining (float time)
	{
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

	public IEnumerator ShowResults()
	{
		yield return new WaitForSeconds (2f);
		resultsWindow.SetActive (true);
		hud.SetActive (false);

		resultsScore.text = _currentScore.ToString ();
		resultsObjective.text = _currentObjective.ToString();

		//needs time bonus calculation
		timeBonus.text = _timeLeft.ToString ();
	}

	void OnContinue()
	{
		SceneManager.LoadScene ("Start");
	}

}
