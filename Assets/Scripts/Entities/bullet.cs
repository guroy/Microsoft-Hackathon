using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {
	public int damage;
	public  Transform explosion;
	public Transform creator;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other)
	{
//		GameObject particles = Instantiate (explosion, this.transform.position, this.transform.rotation) as GameObject;
//		other.gameObject.GetComponent<playerShip>().hull -= damage;
////		Debug.Log (other);
//		Destroy (this.gameObject);
	}
	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform != creator)
		{
	//		collision.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
	//		collision.gameObject.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
			ContactPoint contact = collision.contacts [0];
			Quaternion rot = Quaternion.FromToRotation (Vector3.up, contact.normal);
			Vector3 pos = contact.point;
			Debug.Log (pos);
			Instantiate (explosion, pos, rot);
			collision.gameObject.GetComponent<playerShip>().hull -= damage;
			Destroy (this.gameObject);
		}
	}
}
