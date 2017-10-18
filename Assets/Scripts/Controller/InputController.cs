/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	Vector2 touchOrigin;
	public Counter counter;

	public delegate void TouchEvent (Vector3 touchPosition);
	public delegate void DragEvent (Vector3 touchPosition);
	public TouchEvent touchEvent;
	public DragEvent dragEvent;

#if UNITY_EDITOR || UNITY_WEBGL
	void FixedUpdate ()
	{
		onReceiveController ();
	}
#endif

	public virtual void OnPointerDown (PointerEventData eventData)
	{
	}

	public virtual void OnPointerUp (PointerEventData eventData)
	{
		OnTouch (eventData.position);
	}

	public virtual void OnDrag (PointerEventData eventData)
	{
		OnDrag (eventData.position);
	}

	void onReceiveController ()
	{
		if (Input.mousePresent) {
			if (counter.CurrentState == Counter.CounterState.STOP) {
				OnDrag (Input.mousePosition);
			}
		}
	}

	public void OnTouch (Vector3 position)
	{
		//Debug.Log("Touch " + position);
		if (touchEvent != null) {
			touchEvent (position);
		}
	}

	public void OnDrag (Vector3 position)
	{
		//Debug.Log("Drag " + position);
		if (dragEvent != null) {
			dragEvent (position);
		}
	}

	#region Register event
	public void RegisterEventTouch (TouchEvent teventFunc)
	{
		touchEvent += teventFunc;
	}

	public void RegisterEventDrag (DragEvent deventFunc)
	{
		dragEvent += deventFunc;
	}

	public void UnRegisterEventTouch (TouchEvent teventFunc)
	{
		touchEvent -= teventFunc;
	}

	public void UnRegisterEventDrag (DragEvent deventFunc)
	{
		dragEvent -= deventFunc;
	}
	#endregion
}
