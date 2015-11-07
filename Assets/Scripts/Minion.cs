using UnityEngine;
using System.Collections;
using System;

//use the Generic system here to make use of a Flocker list later on
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]

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

    //Forces
    public float weightSteer;
    public float weightAvoid;
    public float maxSpeed;
    public float maxForce;

    //Collision
    public float fireRange;
    public float safeDist;
    public float radius;
    public List<GameObject> Obstacles;

    //access to character Controller component
    //CharacterController charController;
    
    //Target
    public GameObject seekerTarget;

    public Vector3 Velocity
    {
        get { return velocity; }
    }

	// Use this for initialization
	void Start () 
    {
        //charController = GetComponent<CharacterController>();
       
        //Movement
        acceleration = Vector3.zero;
        velocity = transform.forward;
        desired = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () 
    {
        CalcSteeringForces();
        velocity = Vector3.ClampMagnitude((velocity + acceleration), maxSpeed);
        //Smooth rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        transform.Translate(transform.forward * velocity.magnitude * Time.deltaTime);

        acceleration = Vector3.zero;
        
	}

    //----------------------------------------------------------------------
    // Other Methods
    //----------------------------------------------------------------------



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

        foreach(GameObject o in obstacles)
        {
            steer += avoidObstacle(o);
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
        distance  = distance - ((radius * transform.lossyScale.magnitude) + radiusObst);
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

                    steer = (desVel - velocity);
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
