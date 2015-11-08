using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour 
{
	public int damage;
	public  Transform explosion;
	public Transform creator;
	public int range;
	Vector3 startPos;
    private float timer;
	// Use this for initialization
	void Start () 
    {
		startPos = this.transform.position;
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Mathf.Abs(Vector3.Distance(startPos, this.transform.position)) > range || timer > 10)
        {
            Destroy(this.gameObject);
        }
        else
        {
            timer += Time.deltaTime;
        }
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

            string player;
            string minion;
            string laser;
            if(gameObject.tag == "RedLaser")
            {
                player = "BlueLaser";
                minion = "BlueTeam";
                laser = "BlueLaser";

            }
            else
            {
                player = "RedLaser";
                minion = "RedTeam";
                laser = "RedLaser";
            }

            if ((collision.gameObject.tag == player || collision.gameObject.tag == minion) && collision.gameObject.tag != laser)
			{
				collision.gameObject.GetComponent<playerShip>().hull -= damage;
				collision.gameObject.GetComponent<playerShip>().lastHit = creator.parent.gameObject;
			
			}
			Destroy (this.gameObject);
		}
	}
}
