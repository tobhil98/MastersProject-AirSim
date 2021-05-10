using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AirSimUnity.CarStructs;
using AirSimUnity;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;


[RequireComponent(typeof(AirSimCarController))]
public class MLCar : Agent
{
    /*
     * Car component that is used to control the car object in the scene. A sub class of Vehicle that is communicating with AirLib.
     * This class depends on the AirSimController component to move the car based on Unity physics.
     * The car can be controlled either through keyboard or through client api calls.
     * This class holds the current car state and data for client to query at any point of time.
     */
    private AirSimCarController carController;
    private SimpleControls carController2;

    private CarControls carControls;
    private CarState carState;
    private CarData carData;


    private float steering, throttle, footBreak, handBrake;

    private bool destroySelf_ = false;

    private Vector3 startPos;
    private Quaternion startRotation;

    private float optimalDistance;
    private bool moved = false;
    private int stuckCount = 0;

    private Transform target;

    List<Transform> targetList;

    private int targetIndex;

    private Rigidbody r;

    public override void OnActionReceived(ActionBuffers actions)
    {
        //carControls.handbrake = actions.DiscreteActions[0]>0 ? true : false;
        carControls.throttle = actions.ContinuousActions[0];
        carControls.brake = actions.ContinuousActions[0];
        carControls.steering = actions.ContinuousActions[1]*1.2f;

    }
/*    public override void OnActionReceived(float[] vectorAction)
    {
        carControls.throttle = vectorAction[0];
        carControls.brake = vectorAction[0];
        carControls.steering = vectorAction[1];
    }*/

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 v = target.position - transform.position;
        float directionDot = (float)System.Math.Tanh(Vector3.Angle(transform.forward.normalized, v)/180f);
        sensor.AddObservation(directionDot);
        float turn = AngleDir(transform.forward, v, transform.up);
        sensor.AddObservation(turn);
        //Debug.Log("Observation: " + directionDot + " - " + turn);
    }

    public override void OnEpisodeBegin()
    {
        targetIndex = 0;
        target = targetList[0];

        r.velocity = Vector3.zero;
        r.angularVelocity = Vector3.zero;

        transform.localPosition = startPos;
        transform.rotation = startRotation;

        transform.localPosition += new Vector3(Random.Range(-8, 8), 0, Random.Range(-8, 8));
        transform.Rotate(0, Random.Range(-30, 30), 0, Space.Self);
        //target.localPosition = new Vector3(Random.Range(-40, 40), target.localPosition.y, Random.Range(20, 40));

        optimalDistance = Vector3.Distance(transform.position, target.position);

        SetReward(0);
        moved = false;

    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {


        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        //ActionSegment<int> descreteActions = actionsOut.DiscreteActions;
        continousActions[0] = Input.GetAxis("Vertical");
        //continousActions[1] = Input.GetAxis("Vertical");
        continousActions[1] = Input.GetAxis("Horizontal");
        //descreteActions[0] = Input.GetAxis("Jump")>0 ? 1 : 0;
    }

/*    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Vertical");
        actionsOut[1] = Input.GetAxis("Horizontal");
    }
*/

    private void Start()
    {
        Debug.LogError("Starting now!");
        //carController = transform.GetComponent<AirSimCarController>();
        carController2 = transform.GetComponent<SimpleControls>();
        carControls.Reset();
        startPos = transform.localPosition;
        startRotation = transform.rotation;
        //Debug.LogWarning("A warning assigned to this transform!");
        r = transform.GetComponent<Rigidbody>();


    }


    public Transform checkpointsTransform;
    private void Awake()
    {
        targetList = new List<Transform>();
        foreach (Transform t in checkpointsTransform)
        {
            targetList.Add(t);
        }
    }

    public void FixedUpdate()
    {
        throttle = carControls.throttle;
        handBrake = 0; //carControls.handbrake ? 1 : 0;
        footBreak = carControls.brake;
        steering = carControls.steering;

        //Debug.Log("Throttle: " + throttle + " - Steering: " + steering);

        carController2.Move(steering, throttle);
        //carController.Move(steering, throttle, footBreak, handBrake);
        /*carController.UpdateCarData(ref carData);
        carData.throttle = throttle;
        carData.brake = footBreak;
        carData.steering = steering;*/

        /*Debug.LogWarning("HandBreak: " + handBrake);
        Debug.LogWarning("Steering: " + steering);*/

        if (Vector3.Distance(transform.position, target.position) < optimalDistance-1)
        {
            moved = true;
            stuckCount = 0;
            AddReward(0.007f);
            optimalDistance = Vector3.Distance(transform.position, target.position);
            
            Vector3 v = Vector3.Scale((target.position - transform.position), new Vector3(1, 0, 1)).normalized;
            float a = Vector3.Angle(Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized, v);

            if (a < 10)
            {
                if (throttle > 0.2)
                {
                    AddReward((10 - a) / 400);
                }
            }
            
        }
        else
        {
            if (throttle < 0.2)
            {
                AddReward(-0.003f);
            }
        }

        if (Vector3.Magnitude(r.velocity) > 7)
        {
            AddReward(0.003f);
        }
        else if (Vector3.Magnitude(r.velocity) < 3)
        {
            AddReward(-0.002f);
        }

/*        if (StepCount > 1000 && moved == false)
        {
            //PrefabUtility.RevertObjectOverride(gameObject);
            *//*            Destroy(gameObject);
                        controller.resetMap();*//*
            Debug.LogWarning("StuckCount " + ++stuckCount);
            EpisodeInterrupted();
        }*/

        if (Vector3.Distance(transform.position, target.position) < 12)
        {
            AddReward(3f);
            targetIndex = (targetIndex + 1) % targetList.Count;

            if (targetIndex == 0)
            {
                AddReward(2f);
            }

            target = targetList[targetIndex];
            optimalDistance = Vector3.Distance(transform.position, target.position);
            //EndEpisode();
        }
        //Debug.Log(GetCumulativeReward());
        if(GetCumulativeReward() < -8)
        {
            EndEpisode();
        }

       }

    public void ObjectCollided(Collider other)
    {
        AddReward(-12f);
        EndEpisode();
    }

    private float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return 0f;
        }
        else
        {
            return 0.5f;
        }
    }


}