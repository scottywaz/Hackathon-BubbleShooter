﻿/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail
 *******************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Security.Policy;
using UnityEngine.UI;

public class AimingShotLine : MonoBehaviour
{

    public Transform gun;
    public GameObject prefab;
    public Transform linePivot;
	public LineRenderer lineRender;

    List<GameObject> lineSegments = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
    }
	
    // Update is called once per frame
    void Update()
    {
        calculateLineBasedPhysics();
    }

    void calculateLineBasedPhysics()
    {
        // get list of collision point 
        Ray2D ray = new Ray2D(gun.position, gun.up);
        List<Vector2> listHitWalls = new List<Vector2>();
        List<Vector3> listHitWallsLocal = new List<Vector3>();
        listHitWalls.Add(ray.origin);
        listHitWalls.AddRange(raycastRecursive(ray));
        listHitWalls.ForEach(delegate(Vector2 v)
            {
                listHitWallsLocal.Add(linePivot.InverseTransformPoint(v));
            });

		UpdateLineRendererPoints (listHitWallsLocal);
        // render texture based list 
        //prepareLines(listHitWallsLocal.Count);
        for (int i = 0; i < listHitWallsLocal.Count-1; i++)
        {
            //drawTextureBasedLine(lineSegments[i], listHitWallsLocal[i], listHitWallsLocal[i+1]);
        }
    }

    List<Vector2> raycastRecursive(Ray2D ray, bool wallAlreadyHit = false)
    {
        List<Vector2> list = new List<Vector2>();
        RaycastHit2D hitWall = Physics2D.Raycast(ray.origin, ray.direction,
                                   1080, 1 << LayerMask.NameToLayer(Common.LAYER_WALL_LINE));
        RaycastHit2D hitBall = Physics2D.Raycast(ray.origin, ray.direction,
                                   1080, 1 << LayerMask.NameToLayer(Common.LAYER_BALL));


        if (hitBall.collider != null || ray.direction.Equals(Vector2.zero))
        {
			Vector2 nextPoint = hitBall.point;
			int dist = wallAlreadyHit ? 175 : 500;

			if(Mathf.Abs(Vector2.Distance(ray.origin, hitBall.point)) > dist)
			{
				nextPoint = ray.GetPoint(dist);
			}

			Debug.DrawLine(ray.origin, nextPoint, Color.red);
			list.Add(nextPoint);
            return list;
        }
        if (hitWall.collider != null)
        {
            Debug.DrawLine(ray.origin, hitWall.point, Color.red);
            Vector2 oppositePoint = findOppositePoint(ray.origin, hitWall.point);
            Vector2 dir = oppositePoint - hitWall.point;
            list.Add(hitWall.point);
            //Debug.DrawRay(hitWall.point+dir.normalized,dir*800, Color.blue);
            list.AddRange(raycastRecursive(new Ray2D(hitWall.point + dir.normalized, dir), true));
        }
        return list;
    }

    Vector2 findOppositePoint(Vector2 p, Vector2 pline)
    {
        Vector2 midPoint = new Vector2(p.x, pline.y);
        return midPoint * 2 - p;
    }

    void prepareLines(int count)
    {
        int needToAdd = count - lineSegments.Count;
        for (int i = 0; i < needToAdd; i++)
        {
            GameObject go = GameObject.Instantiate(prefab);
            go.transform.parent = linePivot;
            go.transform.localScale = Vector3.one;
            lineSegments.Add(go);
        }
        lineSegments.ForEach(delegate(GameObject obj)
            {
                obj.SetActive(false);
            });
    }

    void drawTextureBasedLine(GameObject texture, Vector3 start, Vector3 end)
    {
        texture.transform.localPosition = (start + end) / 2;
        texture.transform.up = start - end;
        texture.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(33, (end - start).magnitude);
        texture.SetActive(true);    
    }

	List<Vector3> RemoveDuplicatePoints(List<Vector3> points)
	{
		List<Vector3> newList = new List<Vector3>();

		for (int i = 0; i < points.Count; i++)
		{
			if (!newList.Contains(points[i]))
			{
				newList.Add(points[i]);
			}
		}

		return newList;
	}

	void UpdateLineRendererPoints(List<Vector3> points)
	{
		List<Vector3> linePoints = RemoveDuplicatePoints(points);
		lineRender.numPositions = linePoints.Count;

		for (int i = 0; i < linePoints.Count; i++)
		{
			Vector3 point = new Vector3 (linePoints[i].x,linePoints [i].y, -20f);
			lineRender.SetPosition (i, point);
		}
	}
}
