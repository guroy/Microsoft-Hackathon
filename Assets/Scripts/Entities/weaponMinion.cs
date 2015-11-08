using UnityEngine;
using System.Collections;

public class weaponMinion : MonoBehaviour {

	public string name;
	public int dmgValue;
	public int range;
	public int money;
	public GameObject projectile;
	private Transform spawnpoint;


    public bool fire;
    private float timer;
    private float wait = 2; 

	// Use this for initialization
	void Start () 
    {
        fire = false;
        timer = 0;
        //spawnpoint = GetComponentInChildren<Transform>();
    }
	
	// Update is called once per frame
	void Update () 
    {
		if (fire && timer >= wait) 
		{
			GameObject projectileA = Instantiate (projectile, transform.position + transform.up * 4 , transform.rotation) as GameObject;
			projectileA.GetComponent<bullet>().damage = dmgValue;
			projectileA.GetComponent<bullet>().creator = this.transform;
            string tag;
            if(this.transform.parent.gameObject.tag == "RedTeam")
            {
                tag = "RedLaser";
            }
            else
            {
                tag = "BlueLaser";
            }
            projectileA.tag = tag;
			projectileA.GetComponent<bullet>().range =range;
			projectileA.GetComponent<Rigidbody> ().velocity = transform.TransformDirection (new Vector3 (0, 25, 0));
            timer = 0;
		}
        else 
        {
            timer += Time.deltaTime;
        }
	}
}
