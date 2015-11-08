using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {
	public int damage;
	public  Transform explosion;
	public Transform creator;
	public int range;
	Vector3 startPos;
	// Use this for initialization
	void Start () {
		startPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs(Vector3.Distance (startPos, this.transform.position)) > range)
			Destroy (this.gameObject);
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
		if (collision.transform != creator )
		{
	//		collision.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
	//		collision.gameObject.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
			ContactPoint contact = collision.contacts [0];
			Quaternion rot = Quaternion.FromToRotation (Vector3.up, contact.normal);
			Vector3 pos = contact.point;
			Instantiate (explosion, pos, rot);

			if(collision.gameObject.tag == "bluePlayer" && gameObject.tag == "redPlayer")
			{
				collision.gameObject.GetComponent<playerShip>().hull -= damage;
				collision.gameObject.GetComponent<playerShip>().lastHit = creator.parent.gameObject;
			
			}

			Destroy (this.gameObject);
		}
	}
}
