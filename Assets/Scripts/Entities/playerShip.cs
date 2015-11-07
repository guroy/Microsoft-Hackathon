using UnityEngine;
using System.Collections;

public class playerShip : MonoBehaviour {
	public int hull;
	public int money;
	public int value;
	public GameObject[] bays;
	public Transform explosion;
	public GameObject lastHit;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if (hull <= 0)
		{
			shipExplosion();
		}
	}

	void OnDestroy()
	{

	}
	
	void shipExplosion ()
	{
		Instantiate (explosion, this.transform.position, this.transform.rotation);
		lastHit.GetComponent<playerShip>().money += value;
		Destroy(this.gameObject);
	}
}
