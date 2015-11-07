using UnityEngine;
using System.Collections;

public class ShipEvents : MonoBehaviour {
    private Vector3 screenPoint;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown() {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        // store initial position of object
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseUp() {
        Debug.Log("Pew Pew Pwe");
    }

    void OnMouseDrag() {
        Debug.Log("Moving ship " + Input.mousePosition.x + "," + Input.mousePosition.y + "," + Input.mousePosition.z);
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;

        //TODO: ajouter latence en fonction de la vitesse du drag

    }
}
