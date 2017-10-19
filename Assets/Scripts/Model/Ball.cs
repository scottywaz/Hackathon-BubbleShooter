/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Ball : MonoBehaviour
{

	public Image sprite;

	public Sprite [] ballSprites = new Sprite [6];

	Common.BallColors _color;
	public Common.BallColors GetBallColor ()
	{
		return _color;
	}
	public void SetBallColor (Common.BallColors color)
	{
		_color = color;
		sprite.sprite = getRealColor (color);
	}

	GridCell _gridPosition;
	public GridCell GetGridPosition ()
	{
		return _gridPosition;
	}
	public void SetGridPosition (GridCell grid)
	{
		_gridPosition = grid;
	}

	private Rigidbody2D _rigidBody;
	private bool _isMoving = false;
	private BallManager _ballManager;
	private Counter _counter;


	void Awake ()
	{
		_rigidBody = GetComponent<Rigidbody2D> ();
		_counter = GetComponent<Counter> ();
	}

	void Update ()
	{

	}

	public void Init (BallManager ballManager)
	{
		_ballManager = ballManager;
	}

	public void FixPosition ()
	{
		_isMoving = false;
#if UNITY_5_5_OR_NEWER
		_rigidBody.bodyType = RigidbodyType2D.Static;
#else
        _rigidBody.isKinematic = true;
#endif
	}

	public void AssignBulletToGrid (GridCell gridClue)
	{
		_ballManager.AssignBulletToGrid (this, gridClue);
	}

	public void AssignBulletToGrid (Vector3 position)
	{
		_ballManager.AssignBulletToGrid (this, position);
	}

	public void RemoveBall ()
	{
		Destroy (gameObject);
	}

	public void SetGravity ()
	{
#if UNITY_5_5_OR_NEWER
		_rigidBody.bodyType = RigidbodyType2D.Dynamic;
#else
        _rigidBody.isKinematic = false;
#endif
		_rigidBody.gravityScale = 100;
	}

	Sprite getRealColor (Common.BallColors ballColor)
	{
		Sprite colorResult = sprite.sprite;
		switch (ballColor) {
		case Common.BallColors.Blue:
			colorResult = ballSprites[0];
			break;
		case Common.BallColors.Green:
			colorResult = ballSprites [1];
			break;
		case Common.BallColors.Red:
			colorResult = ballSprites [2];
			break;
		case Common.BallColors.Yellow:
			colorResult = ballSprites [3];
			break;
		case Common.BallColors.Pink:
			colorResult = ballSprites [4];
			break;
		case Common.BallColors.Orange:
			colorResult = ballSprites [5];
			break;
		}

		return colorResult;
	}

	public void WasShoot (Transform bulletRoot, Vector3 force)
	{
		releaseFromGun (bulletRoot);
		addForce (force);
	}

	void releaseFromGun (Transform bulletRoot)
	{
		transform.parent = bulletRoot;
	}

	void addForce (Vector3 force)
	{
		_rigidBody.AddForce (new Vector2 (force.x, force.y), ForceMode2D.Force);
		_isMoving = true;
	}

	public void SetNewLayer (string newLayer)
	{
		gameObject.layer = LayerMask.NameToLayer (newLayer);
		Collider2D collider = GetComponent<Collider2D> ();
		collider.enabled = false;
		collider.enabled = true;
	}

	public void EffectFallingBall ()
	{
		SetNewLayer (Common.LAYER_NONE);
		SetGravity ();
		_counter.StartTimerUpdatePercentage (2, () => {
			RemoveBall ();
		}, null);

	}

	public void EffectExplodeBall ()
	{
		SetNewLayer (Common.LAYER_NONE);
		SetGravity ();
		WasShoot (transform.parent, new Vector3 (Random.Range (-2000, 2000), Random.Range (-2000, 2000), 0));
		_counter.StartTimerUpdatePercentage (2, () => {
			RemoveBall ();
		}, null);
	}

	public void OnCollisionEnter2D (Collision2D other)
	{
		if (_isMoving && gameObject.tag.Equals (Common.LAYER_BULLET)) 
		{
			string nameHit = other.gameObject.tag;
			if (nameHit.Equals (Common.LAYER_BALL) || nameHit.Equals (Common.LAYER_WALL)) 
			{
				gameObject.tag = Common.LAYER_BALL;
				SetNewLayer (Common.LAYER_BALL);
				FixPosition ();

				if (nameHit.Equals (Common.LAYER_BALL)) 
				{
					AssignBulletToGrid (other.gameObject.GetComponent<Ball> ().GetGridPosition ());
				} 
				else 
				{
					AssignBulletToGrid (other.transform.localPosition);
				}

				_ballManager.ExplodeSameColorBall (this);
			}

		}
	}

}
