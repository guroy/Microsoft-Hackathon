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

    protected bool destroyer1;
    protected bool destroyer2;
    protected bool destroyer3;
    protected bool mothership;

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
    }

    void spawnMinions()
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

        GameObject go;
        for (int i = 0; i < 5; i++)
        {
            go = (GameObject)Instantiate(minionPrefab, Spawn.transform.position - Spawn.transform.right * i * 2, Quaternion.identity);
            go.GetComponent<Minion>().seekerTarget = target3;
            MinionsWave3.Add(go);
            go = (GameObject)Instantiate(minionPrefab, Spawn.transform.position + Spawn.transform.forward * i * 2, Quaternion.identity);
            go.GetComponent<Minion>().seekerTarget = target2;
            MinionsWave2.Add(go);
            go = (GameObject)Instantiate(minionPrefab, Spawn.transform.position + Spawn.transform.right * i * 2, Quaternion.identity);
            go.GetComponent<Minion>().seekerTarget = target1;
            MinionsWave1.Add(go);
        }


    }
}
