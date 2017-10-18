/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using System;
[Serializable]
public class LevelProfile
{
	private int _numberOfDifferentColors;
	public int GetNumColor()
	{
		return 6;
	}

	public int numRows = 20;
	public int extraRows = 7;
}
