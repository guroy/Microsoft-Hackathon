using UnityEngine;
using System.Collections;

public class autoDestroy : MonoBehaviour {
	ParticleSystem ps;
	// Use this for initialization
	void Start () {
		ps = this.GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!ps.IsAlive())
			Destroy (ps.gameObject);
	}
}
