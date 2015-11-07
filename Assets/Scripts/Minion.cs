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
       // scanObstacles();
        CalcSteeringForces();
        velocity = Vector3.ClampMagnitude((velocity + acceleration), maxSpeed);

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

    public void separate(List<GameObject> minions)
    {
        Vector3 steer = Vector3.zero;
        float desiredSeparation = radius * transform.lossyScale.magnitude * 4;
        Vector3 sum = new Vector3();
        int count = 0;
        foreach (GameObject min in minions)
        {
            float d = Vector3.Distance(transform.position, min.transform.position);
            if ((d > 0) && (d < desiredSeparation))
            {
                Vector3 diff = transform.position - min.transform.position;
                diff = diff.normalized / d;
                sum += diff;
                count++;
            }
        }
        if (count > 0)
        {
            sum /= count;
            sum = sum.normalized * maxSpeed;
            steer = sum - velocity;
        }

        steer *= weightAvoid;
        steer = Vector3.ClampMagnitude(steer, maxForce);
        ApplyForce(steer);
    }

    /// <summary>
    /// Calculate the steering force for the group of dude
    /// </summary>
    protected void CalcSteeringForces()
    {
        Vector3 desSteerForce = seek(seekerTarget.transform.position);
        desired += desSteerForce;
        //Calculate the force to avoid obstacles
        Vector3 avoidance = avoidObstacle();
        //calculate the separation force

        //weight the different forces to Apply to the acceleration
        desired *= weightSteer;
        avoidance *= weightAvoid;

        //limit the force to apply
        desired = Vector3.ClampMagnitude(desired, maxForce);
        avoidance = Vector3.ClampMagnitude(avoidance, maxForce);

        ApplyForce(desired);
        ApplyForce(avoidance);
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
    /// method that calculate the forces to avoid the obstacles
    /// </summary>
    /// <returns> the vector that give the way tpo avoid the obstacles </returns>
    /// 
    private Vector3 avoidObstacle()
    {
        Vector3 steer = Vector3.zero;
        // left raycast
        Ray leftRay = new Ray((transform.position + transform.right * radius * transform.lossyScale.magnitude), transform.forward);
        RaycastHit leftHit;
        bool left = false;
        float leftDist = 0;
        // right raycast
        Ray rightRay = new Ray((transform.position - transform.right * radius * transform.lossyScale.magnitude), transform.forward);
        RaycastHit rightHit;
        bool right = false;
        float rightDist = 0;

        if (Physics.Raycast(leftRay, out leftHit, safeDist*2))
        {
            leftDist = leftHit.distance;
            Debug.Log("left distance " + leftDist);
            left = true;

        }
        if (Physics.Raycast(rightRay, out rightHit, safeDist*2))
        {
            rightDist = rightHit.distance;
            Debug.Log("right distance " + rightDist);
            right = true;
        }

        Vector3 desVel = Vector3.zero;
        float distance = 0;
        //both ray touch something, we need to know where to go
        if (right && left)
        {
            //Debug.Log("both laser touch");
            //Go left !!
            if (rightDist > leftDist)
            {
                desVel = transform.right * -radius;
                distance = leftDist;
            }
            else
            {
                desVel = transform.right * radius;
                distance = rightDist;
            }
        }
        else if (right)
        {
            desVel = transform.right * -radius * 10;
            //Debug.Log("right laser touch");
        }
        else if (left)
        {
            desVel = transform.right * radius * 10;
            //Debug.Log("left laser touch");
        }
        if(distance == 0)
        {
            distance = 1;
        }
        steer = desVel * 2;
        Debug.Log(distance);
        steer = Vector3.ClampMagnitude(steer, maxForce);

        return steer;
    }
}
