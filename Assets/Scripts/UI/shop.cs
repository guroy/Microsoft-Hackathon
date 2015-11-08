using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class shop : MonoBehaviour {

	public playerShip ship;
	public List<GameObject> items;
	public GameObject slots;
	public GameObject itemShop;
	int slotSelect;

	// Use this for initialization
	void Start () {
		updateSlots();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void buyItem (string itemName)
	{
		foreach (GameObject item in items)
		{
			if (item.name == itemName)
			{
				int cost = item.gameObject.GetComponent<weapon>().money;
				if (ship.money >= cost)
				{
					ship.money -= cost;
					ship.bays[slotSelect] = item;
					ship.changeEquipment();
					itemShop.SetActive(false);
					slots.SetActive(true);
					updateSlots();
				} //else display notEnough money panel
			}
		} 
	}

	public void selectSlot(int slotNumber)
	{
		slotSelect = slotNumber;
		slots.SetActive (false);
		itemShop.SetActive (true);
	}

	void updateSlots()
	{

		for(int i=0 ; i<=2; i++)
		{
			if(ship.bays[i] != null)
			{
				GameObject go = ship.bays[i];
				Debug.Log(ship.bays[i]);
				Image img = transform.GetChild(0).GetChild(i).GetChild(1).GetComponent<Image>();
				img.overrideSprite = Resources.Load<Sprite>(go.GetComponent<weapon>().name);
			}
		}
	}

}
