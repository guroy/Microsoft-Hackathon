using UnityEngine;
using System.Collections;
//use the Generic system here to make use of a Flocker list later on
using System.Collections.Generic;

public class MinionManager : MonoBehaviour
{
    //List of minions
    public List<GameObject> MinionsWave1;
    public List<GameObject> MinionsWave2;
    public List<GameObject> MinionsWave3;

    //Target
    public GameObject Destroyer1;
    public GameObject Destroyer2;
    public GameObject Destroyer3;
    public GameObject MotherShip;
    public GameObject Spawn;
    public GameObject minionPrefab;

    public bool destroyer1;
    public bool destroyer2;
    public bool destroyer3;
    public bool mothership;

    //Timer for spawning Minion
    private float lastSpawn;
    private float timer;
    public float waitSpawn;

    // Use this for initialization
    void Start()
    {
        MinionsWave1 = new List<GameObject>();
        MinionsWave2 = new List<GameObject>();
        MinionsWave3 = new List<GameObject>();
        destroyer1 = true;
        destroyer2 = true;
        destroyer3 = true;
        mothership = true;
        timer = 0;
        waitSpawn = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > lastSpawn + waitSpawn)
        {
            lastSpawn = Time.unscaledTime;
            timer = 0;
            spawnMinions();
        }
        else
        {
            timer += Time.deltaTime;
        }
        foreach (GameObject min in MinionsWave1)
        {
            min.GetComponent<Minion>().separate(MinionsWave1);
        }
        foreach (GameObject min in MinionsWave2)
        {
            min.GetComponent<Minion>().separate(MinionsWave2);
        }

        foreach (GameObject min in MinionsWave3)
        {
            min.GetComponent<Minion>().separate(MinionsWave3);
        }

        destroyShip();
    }

    private void destroyShip()
    {
        //destroy ship from wave1
        for(int i = MinionsWave1.Count-1; i >= 0; i--)
        {
            if (MinionsWave1[i].GetComponent<playerShip>().dead)
            {
                Instantiate(MinionsWave1[i].GetComponent<playerShip>().explosion, MinionsWave1[i].GetComponent<playerShip>().transform.position, MinionsWave1[i].GetComponent<playerShip>().transform.rotation);
                MinionsWave1[i].GetComponent<playerShip>().lastHit.GetComponent<playerShip>().money += MinionsWave1[i].GetComponent<playerShip>().value;
                GameObject buf = MinionsWave1[i] as GameObject;
                MinionsWave1.RemoveAt(i);
                Destroy(buf);
            }
        }
        //destroy ship from wave2
        for (int i = MinionsWave2.Count-1; i >= 0; i--)
        {
            if (MinionsWave2[i].GetComponent<playerShip>().dead)
            {
                Instantiate(MinionsWave2[i].GetComponent<playerShip>().explosion, MinionsWave2[i].GetComponent<playerShip>().transform.position, MinionsWave2[i].GetComponent<playerShip>().transform.rotation);
                MinionsWave2[i].GetComponent<playerShip>().lastHit.GetComponent<playerShip>().money += MinionsWave2[i].GetComponent<playerShip>().value;
                GameObject buf = MinionsWave2[i] as GameObject;
                MinionsWave2.RemoveAt(i);
                Destroy(buf);
            }
        }
        //destroy ship from wave3
        for (int i = MinionsWave3.Count-1; i >= 0; i--)
        {
            if (MinionsWave3[i].GetComponent<playerShip>().dead)
            {
                Instantiate(MinionsWave3[i].GetComponent<playerShip>().explosion, MinionsWave3[i].GetComponent<playerShip>().transform.position, MinionsWave3[i].GetComponent<playerShip>().transform.rotation);
                MinionsWave3[i].GetComponent<playerShip>().lastHit.GetComponent<playerShip>().money += MinionsWave3[i].GetComponent<playerShip>().value;
                GameObject buf = MinionsWave3[i] as GameObject;
                MinionsWave3.RemoveAt(i);
                Destroy(buf);
            }
        }
    }

    private void spawnMinions()
    {
        //SpawnMinions for wave 1
        GameObject target1;
        GameObject target2;
        GameObject target3;
        if (destroyer1)
        {
            target1 = Destroyer1;
        }
        else if (destroyer2 && destroyer3)
        {
            // Random attribution on target 2 or 3, if superior to 50 go on 2 otherwise go on 3
            if (Random.Range(0, 100) > 50)
            {
                target1 = Destroyer2;
            }
            else
            {
                target1 = Destroyer3;
            }
        }
        else if (destroyer2)
        {
            target1 = Destroyer2;
        }
        else if (destroyer3)
        {
            target1 = Destroyer3;
        }
        else if (MotherShip)
        {
            target1 = MotherShip;
        }
        else
        {
            target1 = Spawn;
        }
        // minion wave 2
        if (destroyer2)
        {
            target2 = Destroyer2;
        }
        else if (destroyer1 && destroyer3)
        {
            // Random attribution on target 2 or 3, if superior to 50 go on 2 otherwise go on 3
            if (Random.Range(0, 100) > 50)
            {
                target2 = Destroyer1;
            }
            else
            {
                target2 = Destroyer3;
            }
        }
        else if (destroyer1)
        {
            target2 = Destroyer1;
        }
        else if (destroyer3)
        {
            target2 = Destroyer3;
        }
        else if (MotherShip)
        {
            target2 = MotherShip;
        }
        else
        {
            target2 = Spawn;
        }
        // minion Wave 3
        if (destroyer3)
        {
            target3 = Destroyer3;
        }
        else if (destroyer2 && destroyer1)
        {
            // Random attribution on target 2 or 3, if superior to 50 go on 2 otherwise go on 3
            if (Random.Range(0, 100) > 50)
            {
                target3 = Destroyer2;
            }
            else
            {
                target3 = Destroyer1;
            }
        }
        else if (destroyer2)
        {
            target3 = Destroyer2;
        }
        else if (destroyer1)
        {
            target3 = Destroyer1;
        }
        else if (MotherShip)
        {
            target3 = MotherShip;
        }
        else
        {
            target3 = Spawn;
        }

        GameObject go = null;

        for (int i = 0; i < 5; i++)
        {
            //Wave3
            go = (GameObject)Instantiate(minionPrefab, Spawn.transform.position - Spawn.transform.right * i * 2, Quaternion.identity);
            go.GetComponent<Minion>().priorTarget = target3;
            go.GetComponent<Minion>().seekerTarget = target3;
            go.tag = Spawn.gameObject.tag;
            MinionsWave3.Add(go);
            //Wave2
            go = (GameObject)Instantiate(minionPrefab, Spawn.transform.position + Spawn.transform.forward * i * 2, Quaternion.identity);
            go.GetComponent<Minion>().priorTarget = target2;
            go.GetComponent<Minion>().seekerTarget = target2;
            go.tag = Spawn.gameObject.tag;
            MinionsWave2.Add(go);
            //Wave1
            go = (GameObject)Instantiate(minionPrefab, Spawn.transform.position + Spawn.transform.right * i * 2, Quaternion.identity);
            go.GetComponent<Minion>().priorTarget = target1;
            go.GetComponent<Minion>().seekerTarget = target1;
            go.tag = Spawn.gameObject.tag;
            MinionsWave1.Add(go);
        }

    }
}
