using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerShip : MonoBehaviour {
	public int hull;
	public int money;
	public int value;
    public bool dead = false;
	public GameObject[] bays;
	public Transform explosion;
	public GameObject lastHit;
	public Vector3 itemPosition1;
	public Vector3 itemPosition2;
	public Vector3 itemPosition3;
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

	public void changeEquipment ()
	{
		foreach (Transform tr in transform) 
		{
			if (tr != transform)
			{
				Destroy (tr.gameObject);
			}
		}
		foreach (GameObject go in bays) 
		{
			if(go != null)
			{
			GameObject temp = Instantiate(go);
			temp.transform.parent = this.transform;
			temp.transform.position = itemPosition1;
				itemPosition1 = itemPosition2;
				itemPosition2 = itemPosition3;
				itemPosition3 = temp.transform.position;
				temp.GetComponent<weapon>().enabled = true;
			}
		}
	}
}
