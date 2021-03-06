﻿/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallManager : MonoBehaviour
{
	public Transform Deadline;
	public GameObject BallPrefab;
	public Transform PivotGrid;
	public GameManager GameManager;

	GridManager _gridManager;
	int _numberOfDiffColor;
	Vector3 _originalPosition;
	int _bottomRow;
	int _lastRowToPushDown;
	int emptySlotWeight = 0;

	Common.SimpleEvent _clearBallEvent;
	Common.SimpleEventIntegerParams _scoreEvent;
	Score _score;

	private static int CELL_SIZE_X = 105;
	private static int CELL_SIZE_Y = 96;

	// Use this for initialization
	void Start()
	{
		_originalPosition = PivotGrid.localPosition;
		_score = new Score();
	}

	// Update is called once per frame
	void Update()
	{
	}

	#region Logic ball

	public void InitBalls(LevelProfile level)
	{
		if (_gridManager == null)
		{
			_gridManager = new GridManager(12, level.numRows + level.extraRows, CELL_SIZE_X, CELL_SIZE_Y);
			_numberOfDiffColor = level.GetNumColor();
		}

		_bottomRow = level.numRows - 1;
		_lastRowToPushDown = Mathf.FloorToInt((850 - Deadline.transform.localPosition.y)/CELL_SIZE_Y) - level.extraRows;
		PivotGrid.localPosition = new Vector2(PivotGrid.localPosition.x, (Deadline.localPosition.y + ((level.numRows + level.extraRows - 1) * CELL_SIZE_Y)) + 10);

		for (int i = 1; i < _gridManager.GetGridSizeX()-1; i++)
		{
			for (int j = 0; j < level.numRows; j++)
			{
				if (_gridManager.IsValidGridPosition(i, j))
				{
                    Ball ball = instantiateNewBall(randomBallColor());
					assignBallToGrid(ball, i, j);
					if (ball != null)
					{
                    ball.FixPosition();
					}
				}
			}
		}
	}

	public Ball GenerateBallAsBullet()
	{
		Common.BallColors randomColor = (Common.BallColors)Random.Range(1, _numberOfDiffColor + 1);
		while (randomColor == Common.BallColors.Empty)
		{
			randomColor = (Common.BallColors)Random.Range (1, _numberOfDiffColor + 1);
		}
		Ball ball = instantiateNewBall(randomColor);
		ball.tag = Common.LAYER_BULLET;
		ball.SetNewLayer(Common.LAYER_BULLET);
		return ball;
	}

	Ball instantiateNewBall(Common.BallColors color)
	{
		if (color == Common.BallColors.Empty)
			return null;

		GameObject go = GameObject.Instantiate(BallPrefab);
		go.transform.parent = PivotGrid;
		go.transform.localScale = Vector3.one;
		go.transform.localPosition = new Vector3(0, 0, 0);

		Ball ball = go.GetComponent<Ball>();
		ball.Init(this);
		ball.SetBallColor(color);

		return ball;
	}

	void assignBallToGrid(Ball ball, int x, int y)
	{

        GridCell grid = _gridManager.RegisterBall(x, y, ball);

		if (ball != null)
		{
        ball.SetGridPosition(grid);

        ball.transform.localPosition = ball.GetGridPosition().Position;
        ball.name = "Ball_" + x.ToString() + y.ToString();
		}
	}

	Common.BallColors randomBallColor ()
	{
		List<Common.BallColors> weightedBallColors = new List<Common.BallColors> ();

		weightedBallColors.Add (Common.BallColors.Blue);
		weightedBallColors.Add (Common.BallColors.Green);
		weightedBallColors.Add (Common.BallColors.Orange);
		weightedBallColors.Add (Common.BallColors.Pink);
		weightedBallColors.Add (Common.BallColors.Red);
		weightedBallColors.Add (Common.BallColors.Yellow);

		for (int i = 0; i < emptySlotWeight; i++) {
			weightedBallColors.Add (Common.BallColors.Empty);
		}

		return weightedBallColors [Random.Range (0, weightedBallColors.Count)];
	}

	Common.BallColors getBallColorsFixGeneration(int x, int y)
	{
		Common.BallColors color = Common.BallColors.None;
		if (y < 2)
		{
			if (x < 2)
			{
				color = Common.BallColors.Blue;
			}
			else if (x < 4)
			{
				color = Common.BallColors.Green;
			}
			else if (x < 6)
			{
				color = Common.BallColors.Yellow;
			}
			else if (x < 8)
			{
				color = Common.BallColors.Red;
			}
		}
		else
		{
			if (x < 2)
			{
				color = Common.BallColors.Red;
			}
			else if (x < 4)
			{
				color = Common.BallColors.Yellow;
			}
			else if (x < 6)
			{
				color = Common.BallColors.Blue;
			}
			else if (x < 8)
			{
				color = Common.BallColors.Green;
			}
		}
		return color;
	}

	public void AssignBulletToGrid(Ball bullet, Vector3 position)
	{
		GameManager.gun.UnBlockGun();
		bullet.transform.parent = PivotGrid;
		bullet.transform.localScale = Vector3.one;

		GridCell nearestCell = _gridManager.FindNearestGridCell(bullet.transform.localPosition);
		assignBallToGrid(bullet, nearestCell.X, nearestCell.Y);
	}

	public void AssignBulletToGrid(Ball bullet, GridCell gridCellClue)
	{
		GameManager.gun.UnBlockGun();
		bullet.transform.parent = PivotGrid;
		bullet.transform.localScale = Vector3.one;

		GridCell nearestCell = _gridManager.FindNearestGridCell(gridCellClue, bullet.transform.localPosition);
		assignBallToGrid(bullet, nearestCell.X, nearestCell.Y);
	}

	void removeBallFromGrid(GridCell cell)
	{
		_gridManager.RemoveBallFromGridCell(cell);
	}

	void removeBallFromGame(Ball ball)
	{
		if (ball != null)
			ball.RemoveBall();
	}

	int removeAllUnHoldBall()
	{
		List<Ball> listUnHoldBalls = _gridManager.GetListUnHoldBallsAndUnHoldFromGrid();

		foreach (Ball b in listUnHoldBalls)
		{
			b.EffectFallingBall();
		}
		return listUnHoldBalls.Count;
	}

	#endregion

	#region Actions and Events
	public void ExplodeSameColorBall(Ball ball, int wallHits)
	{
		if (checkAndExplodeSameColorBall(ball, wallHits))
		{
			AudioManager.Instance.PlaySound(AudioManager.Instance.success);
			checkClearBalls();
		}
	}

	public bool checkAndExplodeSameColorBall(Ball bullet, int wallHits)
	{
		//Debug.Log("Checking...");
		// get list same colors
		List<GridCell> listSameColors = _gridManager.GetListNeighborsSameColorRecursive(bullet);
		bool isExploded = listSameColors.Count >= 2;

		// check explode 
		if (isExploded)
		{
			// remove all same colors
			listSameColors.Add(bullet.GetGridPosition());
			int noBallsSameColor = listSameColors.Count;
			foreach (GridCell cell in listSameColors)
			{
				cell.Ball.EffectExplodeBall();
				removeBallFromGrid(cell);
			}

			// remove unrelated/ unhold balls
			int noBallFallingDown = removeAllUnHoldBall();

			if (_scoreEvent != null)
			{
				int calScore = _score.CalculateScore(noBallsSameColor, noBallFallingDown, wallHits);
				_score.AddBallsBroken(noBallsSameColor + noBallFallingDown);
				_score.SetScore(_score.GetScore() + calScore);
				_scoreEvent(_score.GetScore(), _score.GetBallsBroken());
			}
		}

		return isExploded;
	}

	public float PushDown(int numTimes = 1)
	{
		float heightDown = _gridManager.GetCellSizeY() * numTimes;
		PivotGrid.localPosition -= new Vector3(0, heightDown, 0);
		return heightDown;
	}

	public void ClearBalls()
	{
		for (int i = 0; i < _gridManager.GetGridSizeX(); i++)
		{
			for (int j = 0; j < _gridManager.GetGridSizeY(); j++)
			{
				removeBallFromGame(_gridManager.GetGridCell(i, j).Ball);
				removeBallFromGrid(_gridManager.GetGridCell(i, j));
			}
		}
	}

	int countRemainingBalls()
	{
		int count = 0;
		for (int i = 0; i < _gridManager.GetGridSizeX(); i++)
		{
			for (int j = 0; j < _gridManager.GetGridSizeY(); j++)
			{
				if (_gridManager.IsOccupiedBall(i, j))
					count++;
			}
		}
		return count;
	}

	void checkClearBalls()
	{
		if (countRemainingBalls() == 0)
		{
			if (_clearBallEvent != null)
			{
				_clearBallEvent();
			}
		}
		else // check if the last row is empty
		{
			int rowsRemoved = NumberOfRowsRemoved();
			if(rowsRemoved > 0)
			{
				PushDown(rowsRemoved);
			}
		}
    }

	int NumberOfRowsRemoved(int soFar = 0)
	{
		if (_bottomRow > _lastRowToPushDown)
		{
			if (!_gridManager.IsRowOccupied(_bottomRow))
			{
				soFar++;
				_bottomRow--;
				soFar = NumberOfRowsRemoved(soFar);
			}
		}

		return soFar;
	}

    public void RegisterEventClearBall(Common.SimpleEvent e)
    {
        _clearBallEvent = e;
    }

    public void Reset()
    {
        PivotGrid.localPosition = _originalPosition;
        _score.SetScore(0);
    }

    public void RegisterEventCalculateScore(Common.SimpleEventIntegerParams e)
    {
        _scoreEvent = e;
    }

    #endregion
}
