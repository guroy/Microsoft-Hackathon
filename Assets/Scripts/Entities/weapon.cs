﻿using UnityEngine;
using System.Collections;

public class weapon : MonoBehaviour {

	public string name;
	public int dmgValue;
	public int range;
	public int money;
	public GameObject projectile;
	public Transform spawnpoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("mouse 1")) 
		{
			GameObject projectileA = Instantiate (projectile, spawnpoint.position, spawnpoint.rotation) as GameObject;
			projectileA.GetComponent<bullet>().damage = dmgValue;
			projectileA.GetComponent<bullet>().creator = this.transform;
            string tag;
            if (this.transform.parent.gameObject.tag == "RedTeam" || this.transform.parent.gameObject.tag == "RedPlayer")
            {
                tag = "RedLaser";
            }
            else
            {
                tag = "BlueLaser";
            }
            projectileA.tag = tag;
			projectileA.tag = tag;
			projectileA.GetComponent<bullet>().range =range;
			projectileA.GetComponent<Rigidbody> ().velocity = spawnpoint.TransformDirection (new Vector3 (0, 25, 0));
		}		 
	}
}
