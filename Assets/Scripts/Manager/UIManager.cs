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
        _centerText.gameObject.SetActive(true);
        _centerText.text = "Game Over";
    }

    public void DisplayWin()
    {
        _centerText.gameObject.SetActive(true);
        _centerText.text = "Win";
    }

    public void UpdateScore(int score)
    {
        _score.text = score.ToString();
    }

    public void DisableText()
    {
        _centerText.gameObject.SetActive(false);
    }

    public void TurnOnRedAlert()
    {
    }

    public void NormalMode()
    {
    }

}
