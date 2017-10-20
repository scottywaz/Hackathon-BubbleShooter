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

		//End game if time goes down to 0
		if (time < 0) {

			Debug.Log("Gameover");
			timerEnded ();

		}

		uiManager.UpdateTimeRemaining(time);
	}


	void timerEnded()
		{
		game.OnGameover();
		gameEnded = true;

		}

	}

