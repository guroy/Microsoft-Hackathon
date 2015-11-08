using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class hud : MonoBehaviour {

	public playerShip playerShip;
	public Image hull;
	public Text money;
	private int inititalHull;
	// Use this for initialization
	void Start () {
		inititalHull = playerShip.hull;
	}
	
	// Update is called once per frame
	void Update () {
		money.text = playerShip.money.ToString();
		float percent = (float)playerShip.hull / inititalHull;
		hull.fillAmount = percent;
	}
}
