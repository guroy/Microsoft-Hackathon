using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class proximity : MonoBehaviour {
	public List<GameObject> shops;
	public GameObject shopButton;
	public int proximityDist;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate()
	{
		foreach (GameObject shop in shops)
		{
			int dist = (int) Mathf.Abs(Vector3.Distance(shop.transform.position, transform.position));
			if (dist <= proximityDist)
				shopButton.SetActive(true);
			else shopButton.SetActive(false);
		}
	}
}
