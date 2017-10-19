using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GameTimer: MonoBehaviour 
	{ 
		public UIManager uiManager;
		public GameManager game;
		private float time = 180.00f; //in seconds
		private bool gameEnded = false;


	void Update() {

		if (gameEnded) {
			return;
		}
			
		time -= Time.deltaTime;

		var minutes = time / 60; //Divide the guiTime by sixty to get the minutes.
		var seconds = time % 60;//Use the euclidean division for the seconds.

		//End game if time goes down to 0
		if (time < 0) {

			Debug.Log("Gameover");
			timerEnded ();

		}

		uiManager.UpdateTimeRemaining(string.Format ("{0:00} : {1:00}", minutes, seconds));
	}


	void timerEnded()
		{
		game.OnGameover();
		gameEnded = true;

		}

	}

