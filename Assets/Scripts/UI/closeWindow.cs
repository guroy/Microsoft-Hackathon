using UnityEngine;
using System.Collections;

public class closeWindow : MonoBehaviour {
	public GameObject window;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void closeThat ()
	{
		window.SetActive (false);
	}
}
