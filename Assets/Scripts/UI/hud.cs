using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class hud : MonoBehaviour {

	public GameObject playerShip;
	public Text hull;
	public Text money;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		money.text = playerShip.GetComponent<playerShip>().money.ToString();
		hull.text = "Hull: " + playerShip.GetComponent<playerShip>().hull.ToString();
	
	}
}
