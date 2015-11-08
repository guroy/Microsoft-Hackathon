using UnityEngine;
using System.Collections;

public class playerShip : MonoBehaviour {
	public int hull;
	public int money;
	public int value;
    public bool dead = false;
	public GameObject[] bays;
	public Transform explosion;
	public GameObject lastHit;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
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
        dead = true;
	}
}
