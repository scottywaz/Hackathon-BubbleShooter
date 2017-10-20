/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using System.Collections;

public class Score  
{
    int _score;
	int _ballsBroken = 0;

    public int GetScore()
	{
        return _score;
    }

    public void SetScore(int score)
	{
        _score = score;
    }

	public int GetBallsBroken()
	{
		return _ballsBroken;
	}

	public void AddBallsBroken(int numBalls)
	{
		_ballsBroken += numBalls;
	}
	
    // fomular score scale 
    public int CalculateScore(int pointSameColor, int fallingDown, int wallHits)
	{
		return pointSameColor * 30 + fallingDown * 150 + wallHits * 30; 
    }

}
