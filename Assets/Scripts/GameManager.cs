using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject blueMotherShip;
    private List<Vector3> bluePos;

    public GameObject redMotherShip;
    private List<Vector3> redPos;

    public List<GameObject> turrets;
    public GameObject player;
    private float playerTimer;
    private bool playerWait;
    private int SPAWN_COOLDOWN = 30;
    
    // Use this for initialization
	void Start ()
    {
        //Instantiate the ship
        playerWait = false;
        //player = (GameObject)Instantiate(playerPrefab, blueMotherShip.transform.position, Quaternion.identity);
        //instantiate both mothership
    }
	
	// Update is called once per frame
	void Update ()
    {
        destroyMotherShip();
        playerDead();
        turretDestroy();
        //Respawn our ship
        if(playerWait && playerTimer >= SPAWN_COOLDOWN)
        {
            //playerSpawn(blueMotherShip);
            playerTimer = 0;
            playerWait = false;
        }
        else
        {
            playerTimer += Time.deltaTime;
        }
	}

    /// <summary>
    /// Destroy the player
    /// </summary>
    private void playerDead()
    {
        bool hp = player.GetComponentInChildren<playerShip>().dead;
        if(hp)
        {
            Instantiate(player.GetComponent<playerShip>().explosion, player.GetComponent<playerShip>().transform.position, player.GetComponent<playerShip>().transform.rotation);
            player.GetComponent<playerShip>().lastHit.GetComponent<playerShip>().money += player.GetComponent<playerShip>().value;
            Destroy(player);
            playerWait = true;
        }
    }

    /// <summary>
    /// Player spawn method
    /// </summary>
    /// <param name="motherShip"></param>
    //private void playerSpawn(GameObject motherShip)
    //{

    //    string tag;
    //    if(motherShip.gameObject.tag == "BlueTeam")
    //    {
    //        tag = "BluePlayer";
    //    }
    //    else
    //    {
    //        tag = "RedPlayer";
    //    }
    //    player = (GameObject)Instantiate(playerPrefab, motherShip.transform.position, Quaternion.identity);
    //    player.gameObject.tag = tag;
    //}

    /// <summary>
    /// Destroy the turret that should be
    /// </summary>
    private void turretDestroy()
    {
        for(int i = turrets.Count - 1; i >= 0; i--)
        {
            bool hp = turrets[i].GetComponentInChildren<playerShip>().dead;
            if (hp)
            {
                string name = turrets[i].gameObject.name;
                if (turrets[i].gameObject.tag == "BlueTeam")
                {
                    
                    if(name == "Bdestoyer1")
                    {
                        blueMotherShip.GetComponentInChildren<MinionManager>().destroyer1 = false;
                    }
                    else if(name == "Bdestoyer2")
                    {
                        blueMotherShip.GetComponentInChildren<MinionManager>().destroyer2 = false;
                    }
                    else if(name == "Bdestroyer3")
                    {
                        blueMotherShip.GetComponentInChildren<MinionManager>().destroyer3 = false;
                    }
                }
                else
                {
                    if (name == "Rdestoyer1")
                    {
                        blueMotherShip.GetComponentInChildren<MinionManager>().destroyer1 = false;
                    }
                    else if (name == "Rdestoyer2")
                    {
                        blueMotherShip.GetComponentInChildren<MinionManager>().destroyer2 = false;
                    }
                    else if (name == "Rdestroyer3")
                    {
                        blueMotherShip.GetComponentInChildren<MinionManager>().destroyer3 = false;
                    }
                }

                //Destroy the turret
                Instantiate(turrets[i].GetComponent<playerShip>().explosion, turrets[i].GetComponent<playerShip>().transform.position, turrets[i].GetComponent<playerShip>().transform.rotation);
                turrets[i].GetComponent<playerShip>().lastHit.GetComponent<playerShip>().money += turrets[i].GetComponent<playerShip>().value;
                GameObject buf = turrets[i] as GameObject;
                turrets.RemoveAt(i);
                Destroy(buf);
            }
        }
    }

    private void destroyMotherShip()
    {
        bool hpBlue = blueMotherShip.GetComponentInChildren<playerShip>().dead;
        bool hpRed = redMotherShip.GetComponentInChildren<playerShip>().dead;

        if(hpBlue && hpRed)
        {
            Instantiate(blueMotherShip.GetComponent<playerShip>().explosion, blueMotherShip.GetComponent<playerShip>().transform.position, blueMotherShip.GetComponent<playerShip>().transform.rotation);
            blueMotherShip.GetComponent<playerShip>().lastHit.GetComponent<playerShip>().money += blueMotherShip.GetComponent<playerShip>().value;
            Destroy(blueMotherShip);

            Instantiate(redMotherShip.GetComponent<playerShip>().explosion, redMotherShip.GetComponent<playerShip>().transform.position, redMotherShip.GetComponent<playerShip>().transform.rotation);
            redMotherShip.GetComponent<playerShip>().lastHit.GetComponent<playerShip>().money += redMotherShip.GetComponent<playerShip>().value;
            Destroy(redMotherShip);
        }
        else if(hpBlue)
        {
            Instantiate(blueMotherShip.GetComponent<playerShip>().explosion, blueMotherShip.GetComponent<playerShip>().transform.position, blueMotherShip.GetComponent<playerShip>().transform.rotation);
            blueMotherShip.GetComponent<playerShip>().lastHit.GetComponent<playerShip>().money += blueMotherShip.GetComponent<playerShip>().value;
            Destroy(blueMotherShip);
        }
        else if(hpRed)
        {
            Instantiate(redMotherShip.GetComponent<playerShip>().explosion, redMotherShip.GetComponent<playerShip>().transform.position, redMotherShip.GetComponent<playerShip>().transform.rotation);
            redMotherShip.GetComponent<playerShip>().lastHit.GetComponent<playerShip>().money += redMotherShip.GetComponent<playerShip>().value;
            Destroy(redMotherShip);
        }
    }


}
