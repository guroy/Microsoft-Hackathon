using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{
    private GameObject target;
    private bool targetLock;
    public float fireRange;
    // Use this for initialization
    void Start()
    {
        targetLock = false;
        target = null;
    }

    // Update is called once per frame
    void Update()
    {
        findTarget();
        if(targetLock)
        {
            GetComponentInChildren<weaponMinion>().fire = true;
        }
        
    }

    private bool findTarget()
    {
        if (target == null || (target.transform.position - transform.position).magnitude > fireRange)
        {
            targetLock = false;
        }
        if (!targetLock)
        {
            string tagPlayer;
            string tagMinion;
            if (gameObject.tag == "BlueTurret")
            {
                tagPlayer = "RedPlayer";
                tagMinion = "RedTeam";
            }
            else
            {
                tagPlayer = "BluePlayer";
                tagMinion = "BlueTeam";
            }
            GameObject[] players = GameObject.FindGameObjectsWithTag(tagPlayer);
            float distance = 1000;
            foreach (GameObject player in players)
            {
                float buf = Vector3.Distance(player.transform.position, transform.position);
                if (buf < distance)
                {
                    distance = buf;
                    if (distance <= fireRange)
                    {
                        target = player;
                        targetLock = true;
                    }
                }
            }
            GameObject[] minions = GameObject.FindGameObjectsWithTag(tagMinion);
            foreach(GameObject minion in minions)
            {
                float buf = Vector3.Distance(minion.transform.position, transform.position);
                if(buf < distance)
                {
                    distance = buf;
                    if(distance <= fireRange)
                    {
                        target = minion;
                        targetLock = true;
                    }
                }
            }
        }
        return targetLock;
    }

    /// <summary>
    /// Functionn that allow the head of the turret to follow the target
    /// </summary>
    //private void aim()
    //{

    //}
}

