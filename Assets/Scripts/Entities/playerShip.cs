using UnityEngine;
using System.Collections;

public class playerShip : MonoBehaviour {
	public int hull;
	public GameObject[] bays;
	public Transform explosion;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if (hull <= 0)
		{
			Instantiate (explosion, this.transform.position, this.transform.rotation);
			Destroy(this.gameObject);
		}
	}
}
