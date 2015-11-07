﻿using UnityEngine;
using System.Collections;

public class MiniMap : MonoBehaviour 
{
	public Transform Target;
	public float ZoomLevel=10f;

	Vector2 XRotation = Vector2.right;
	Vector2 YRotation = Vector2.up;

	void LateUpdate(){
		XRotation = new Vector2 (Target.right.x, -Target.right.z);
		YRotation = new Vector2 (-Target.forward.x, Target.forward.z);
	}

	public Vector2 TransformPosition(Vector3 position)
	{
		Vector3 offset = position - Target.position;
		Vector2 newPosition = offset.x * XRotation;
		newPosition += offset.z * YRotation;

		newPosition *= ZoomLevel;

		return newPosition; 
	}

	public Vector3 TransformRotation(Vector3 rotation){
		return new Vector3 (0, 0, Target.eulerAngles.y - rotation.y);
	}

	public Vector2 MoveInside(Vector2 point){
		Rect mapRect = GetComponent<RectTransform> ().rect;
		point = Vector2.Max (point, mapRect.min);
		point = Vector2.Min (point, mapRect.max);
		return point;
	}

}
