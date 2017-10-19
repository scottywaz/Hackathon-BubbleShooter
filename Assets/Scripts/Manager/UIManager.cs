/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Text _centerText;
    public Text _score;
	public Text _timeRemaining;
	public Text _objectiveProgress;


    // Use this for initialization
    void Start()
    {
    }
	
    // Update is called once per frame
    void Update()
    {
	
    }

    public void DisplayGameOver()
    {
		_centerText.transform.parent.gameObject.SetActive(true);
        _centerText.text = "Oh fudge! Game Over";
    }

    public void DisplayWin()
    {
		_centerText.transform.parent.gameObject.SetActive(true);
        _centerText.text = "Win";
    }

    public void UpdateScore(int score)
    {
        _score.text = score.ToString();
    }

	public void UpdateTimeRemaining(int time)
	{
		_timeRemaining.text = time.ToString();
	}

	public void UpdateObjectiveProgress(int objective)
	{
		_objectiveProgress.text = objective.ToString();
	}

    public void DisableText()
    {
		_centerText.transform.parent.gameObject.SetActive(false);
    }

    public void TurnOnRedAlert()
    {
    }

    public void NormalMode()
    {
    }

}
