using UnityEngine;
using System.Collections;

public class dragShip : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 offset;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown ()
	{
		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
//		Debug.Log(screenPoint);
		offset = gameObject.transform.position -Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

	}

	void OnMouseDrag(){
//		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y,screenPoint.z);
//		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint)+offset;
//		Vector3 converter = new Vector3 (cursorPosition.x, 0, cursorPosition.y);
////		Vector3 cursorPosition = Camera.main.ScreenToViewportPoint(cursorPoint) + offset;
////		transform.position = cursorPosition;
//		transform.position = converter;
//		Debug.Log ("why dont u!!!");
		Plane castedPlane = new Plane(Vector3.up, Vector3.zero);

		Ray cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		float rayDist = 100f;
		if (castedPlane.Raycast(cursorRay, out rayDist))
		{
			Vector3 cursorPosition = (cursorRay.GetPoint(rayDist));
			transform.position = cursorPosition;
		}


}

}