using UnityEngine;
using System.Collections;
using System;

//use the Generic system here to make use of a Flocker list later on
using System.Collections.Generic;

public class Minion : MonoBehaviour
{
    //----------------------------------------------------------------------
    // Class Field
    //----------------------------------------------------------------------
    //movement
    protected Vector3 acceleration;
    protected Vector3 velocity;
    protected Vector3 desired;
    public float mass;
    public float rotationSpeed;
    protected bool fire;

    //Forces
    public float weightSteer;
    public float weightAvoid;
    public float maxSpeed;
    public float maxForce;

    //Collision
    public float fireRange;
    public float safeDist;
    public float radius;
    private List<GameObject> Obstacles;

    //access to character Controller component
    //CharacterController charController;

    //Target
    public GameObject seekerTarget;

    public Vector3 Velocity
    {
        get { return velocity; }
    }

    public Minion(GameObject target)
    {
        seekerTarget = target;
    }

    // Use this for initialization
    void Start()
    {
        //charController = GetComponent<CharacterController>();

        //Movement
        acceleration = Vector3.zero;
        velocity = transform.forward;
        desired = Vector3.zero;

        //Fight
        fire = false;

        //Collision
        Obstacles = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        scanObstacles();
        CalcSteeringForces();
        velocity = Vector3.ClampMagnitude((velocity + acceleration), maxSpeed);

        //-------------------------------------------------------------
        // FANCY ROLL STUFF NEED TO BE DONE LATER
        //-------------------------------------------------------------
        //float angle = Vector3.Angle(transform.forward, velocity);
        //Debug.Log(angle);
        //float roll = 0;
        //if(angle < 0)
        //{
        //    roll = -15;
        //    transform.Rotate(0, 0,roll);
        //}
        //else if(angle > 0)
        //{

        //    roll = 15;
        //    transform.Rotate(0,0, roll);
        //}
        //else
        //{
        //    transform.Rotate(0,0,-transform.rotation.z);
        //}


        //Smooth rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), rotationSpeed * Time.deltaTime);
        transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        //Check if the target is in the fire range
        if (isArrived(seekerTarget))
        {
            //FIRE UP 
        }

        //Make the AI go forward
        transform.Translate(Vector3.forward * velocity.magnitude * Time.deltaTime);
        //reset acceleration
        acceleration = Vector3.zero;

    }

    //----------------------------------------------------------------------
    // Other Methods
    //----------------------------------------------------------------------

    protected void separate(List<Minion> minions)
    {
        float desiredSeparation = radius *transform.lossyScale.magnitude  * 2;
        Vector3 sum = new Vector3();
        int count = 0;
        foreach(Minion min in minions)
        {
            float d = Vector3.Distance(transform.position, min.transform.position);
            if((d > 0) && (d <desiredSeparation))
            {
                Vector3 diff = transform.position - min.transform.position;
                diff = diff.normalized / d;
                sum += diff;
                count++;
            }
        }
        if(count > 0)
        {
            sum /= count;
            sum = sum.normalized * maxSpeed;
            Vector3 steer = sum - velocity;
            Vector3.ClampMagnitude(steer, weightAvoid);
            ApplyForce(steer);
        }
    }

    /// <summary>
    /// Calculate the steering force for the group of dude
    /// </summary>
    protected void CalcSteeringForces()
    {
        Vector3 desSteerForce = seek(seekerTarget.transform.position);
        desired += desSteerForce;
        //Calculate the force to avoid obstacles
        Vector3 avoidance = avoidAllObstacles(Obstacles);
        //weight the different forces to Apply to the acceleration
        desired *= weightSteer;
        avoidance *= weightAvoid;
        //limit the force to apply
        desired = Vector3.ClampMagnitude(desired, maxForce);
        avoidance = Vector3.ClampMagnitude(avoidance, maxForce);

        ApplyForce(desired);
        ApplyForce(avoidance);
    }

    protected void scanObstacles()
    {
        //clear the previous list of obstacles
        Obstacles.Clear();
        foreach (GameObject o in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            if (o.tag != "Manager" && o.tag != "MainCamera" && o.tag!= "part")
            {

                Debug.Log(o.tag);
                //check if the object o is ibn the active hierarchy
                //get the distance with the object
                Vector3 vecToC = o.transform.position - transform.position;
                //check if the obecjt is in the dangerous object
                if ((vecToC.magnitude > (radius * transform.lossyScale.magnitude)) && (vecToC.magnitude < safeDist))
                {

                    //add the object to the obstacles list
                    Obstacles.Add(o);
                    Debug.Log(o.name);
                }
            }
        }
    }

    /// <summary>
    /// Fire on the target if it's in the fire range and oriented the ship to the target
    /// </summary>
    /// <param name="target"> the target aim by the AI</param>
    /// <returns>return true if the the target is in the fire range, else return false</returns>
    protected bool isArrived(GameObject target)
    {
        bool res = false;
        Vector3 vecToC = target.transform.position - transform.position;
        //get the distance between the two gameobject, take account of the hitbox
        float distance = vecToC.magnitude - (radius * transform.lossyScale.magnitude) - (target.GetComponent<SphereCollider>().radius * transform.lossyScale.magnitude);
        if (distance <= fireRange)
        {
            velocity = Vector3.zero;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vecToC), rotationSpeed * Time.deltaTime);
            res = true;
        }

        //if ze arrived ze need to shoot
        fire = res;
        return res;
    }

    /// <summary>
    /// Apply the steering force to the acceleration vector
    /// </summary>
    /// <param name="steeringForce">the steering force to applied</param>
    protected void ApplyForce(Vector3 steeringForce)
    {
        acceleration += steeringForce / mass;
    }

    /// <summary>
    /// calculate the seeking force for the vehicle
    /// </summary>
    /// <returns> the vector that give the desired velocity calculate by the seeking force </returns>
    protected Vector3 seek(Vector3 target)
    {
        desired = target - transform.position;
        desired = desired.normalized * maxSpeed;
        desired -= velocity;
        //avoid unwanted take off !
        desired.y = 0;

        return desired;
    }

    /// <summary>
    /// Calculate the final steering force 
    /// </summary>
    /// <param name="obstacles"></param>
    /// <returns></returns>
    protected Vector3 avoidAllObstacles(List<GameObject> obstacles)
    {
        Vector3 steer = Vector3.zero;

        foreach (GameObject o in obstacles)
        {
            Vector3 buf = avoidObstacle(o);
          steer += Vector3.Lerp(steer, buf,1f);
        }
        steer = Vector3.ClampMagnitude(steer, maxForce);
        return steer;
    }

    /// <summary>
    /// method that calculate the forces to avoid the obstacles
    /// </summary>
    /// <returns> the vector that give the way tpo avoid the obstacles </returns>
    private Vector3 avoidObstacle(GameObject obst)
    {
        Vector3 steer = Vector3.zero;
        //Calculate the vector between the current minion and obst
        Vector3 vecToC = transform.position - obst.transform.position;
        //Find the distance between the minion and obst
        float distance = vecToC.magnitude;
        float radiusObst = obst.GetComponent<SphereCollider>().radius * obst.GetComponent<Transform>().lossyScale.magnitude;
        distance = distance - ((radius * transform.lossyScale.magnitude) + radiusObst);
        //If obst is in the safe distance of the minion, he try to avoid by adding a force
        if (distance < safeDist)
        {
            //Test if obst is behind
            if (Vector3.Dot(vecToC, transform.forward) < 0)
            {
                // So far the obstacle is in front of the minion and dangerous
                //We know want to know if obst is on the right or on the left
                float distToC = Vector3.Dot(transform.right, vecToC);
                if ((radius + radiusObst) - Math.Abs(distToC) > 0)
                {
                    //This far mean that obst will lead to a collision
                    //we now need to know if we should go left or right
                    Vector3 desVel = Vector3.zero;
                    //Test if we should go right
                    if (distToC < 0)
                    {
                        desVel = transform.right * -radiusObst;
                    }
                    else // else we should go left
                    {
                        desVel = transform.right * radiusObst;
                    }

                    steer = (desVel - velocity) * (distance);
                    steer = Vector3.ClampMagnitude(steer, maxForce);
                }
            }
        }
        return steer;
    }

    /// <summary>
    /// calculate the fleeing foce for the vehicle
    /// </summary>
    /// <returns></returns>
    //private Vector2 flee()
    //{
    //    return new Vector2();
    //}

    /// <summary>
    /// Calculate movement for the group: acceleration, velocity and desired velocity
    /// </summary>
    //private void movement()
    //{

    //}

    /// <summary>
    ///  Interact zith the GameManager script to get the centroid
    /// </summary>
    //private void getCentroid()
    //{

    //}




}
