using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class displayCurrentMoney : MonoBehaviour {
	public playerShip ship;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		gameObject.GetComponent<Text>().text = ship.money.ToString();
	}
}
