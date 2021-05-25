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

    public Transform target;
    public Transform selfTransfrom;

    // List<Transform> targetList;
    // List<Transform> startList;

    private int targetIndex;

    public Rigidbody Rb;

    public int tag;
    private MLController controller;


    public void Setup(MLController c, int t)
    {
        controller = c;
        tag = t;
    }
    public void SetPosition(Vector3 pos, Quaternion ang)
    {
        transform.position = pos;
        transform.rotation = ang;
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        //carControls.handbrake = actions.DiscreteActions[0]>0 ? true : false;
        /*        carControls.throttle = actions.ContinuousActions[0];
                carControls.brake = actions.ContinuousActions[0];
                carControls.steering = actions.ContinuousActions[1];*/

        float throttle = 0f;
        float turn = 0f;

        switch (actions.DiscreteActions[0])
        {
            case 0: throttle =  0f; break;
            case 1: throttle = +1f; break;
            case 2: throttle = -1f; break;
        }
        
        switch (actions.DiscreteActions[1])
        {
            case 0: turn =  0f; break;
            case 1: turn = +1f; break;
            case 2: turn = -1f; break;
        }


        carControls.throttle = throttle;
        carControls.brake = throttle;
        carControls.steering = turn;

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
        float directionDot = (float)System.Math.Tanh(2.5f*Vector3.Angle(transform.forward.normalized, v)/180f);
        float turn = AngleDir(transform.forward, v, transform.up);
        sensor.AddObservation(directionDot * turn);

        sensor.AddObservation(Vector3.Magnitude(Rb.velocity) / 25.0f);
        //sensor.AddObservation(turn);

        //Debug.Log(Vector3.Magnitude(r.velocity)/25);
/*        if(tag ==1)
        Debug.Log("Observation: " + directionDot + " / " + turn + " / " + directionDot * turn);*/
    }

    /*    public override void OnEpisodeBegin()
        {
            Rb.velocity = Vector3.zero;
            Rb.angularVelocity = Vector3.zero;

            var p = startList[Random.Range(0, startList.Count)].position;

            transform.position = new Vector3(p.x, startPos.y, p.z);
            transform.rotation = startRotation;

            transform.localPosition += new Vector3(Random.Range(-8, 8), 0, Random.Range(-8, 8));
            transform.Rotate(0, Random.Range(-10, 50), 0, Space.Self);
            //target.localPosition = new Vector3(Random.Range(-40, 40), target.localPosition.y, Random.Range(20, 40));


            SetReward(0);
            moved = false;
            Debug.LogWarning(target.name);
        }*/


    public override void OnEpisodeBegin()
    {
        Rb.velocity = Vector3.zero;
        Rb.angularVelocity = Vector3.zero;

        transform.localScale = defaultScale;
        gameObject.SetActive(true);
        optimalDistance = Vector3.Distance(transform.position, target.position);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {


        //ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        //discreteActions[0] = (int)Input.GetAxis("Vertical");
        //continousActions[1] = Input.GetAxis("Vertical");
        //discreteActions[1] = (int)Input.GetAxis("Horizontal");
        //discreteActions[0] = Input.GetAxis("Jump")>0 ? 1 : 0;


        int forward = 0;
        int turn = 0;

        if (tag == 1)
        {
            if (Input.GetKey(KeyCode.UpArrow)) forward = 1;
            if (Input.GetKey(KeyCode.DownArrow)) forward = 2;

            if (Input.GetKey(KeyCode.RightArrow)) turn = 1;
            if (Input.GetKey(KeyCode.LeftArrow)) turn = 2;

        }
        else
        {
            if (Input.GetKey(KeyCode.W)) forward = 1;
            if (Input.GetKey(KeyCode.S)) forward = 2;

            if (Input.GetKey(KeyCode.D)) turn = 1;
            if (Input.GetKey(KeyCode.A)) turn = 2;
        }

        discreteActions[0] = forward;
        discreteActions[1] = turn;
    }

    /*    public override void Heuristic(float[] actionsOut)
        {
            actionsOut[0] = Input.GetAxis("Vertical");
            actionsOut[1] = Input.GetAxis("Horizontal");
        }
    */


    Vector3 defaultScale;
    public override void Initialize()
    {
        base.Initialize();
        //carController = transform.GetComponent<AirSimCarController>();
        carController2 = transform.GetComponent<SimpleControls>();
/*        carControls.Reset();
        startPos = transform.position;
        startRotation = transform.rotation;*/
        //Debug.LogWarning("A warning assigned to this transform!");
        Rb = transform.GetComponent<Rigidbody>();
        selfTransfrom = transform;
        defaultScale = transform.localScale;

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
            AddReward(0.003f);
            optimalDistance = Vector3.Distance(transform.position, target.position);
            
            Vector3 v = Vector3.Scale((target.position - transform.position), new Vector3(1, 0, 1)).normalized;
            float a = Vector3.Angle(Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized, v);

            if (a < 10)
            {
                AddReward((10 - a) / 1000);
            }
            
        }

        if (Vector3.Magnitude(Rb.velocity) > 7)
        {
            AddReward(0.001f);
        }
        else if (Vector3.Magnitude(Rb.velocity) < 3)
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
            controller.CarFinished(this);
        }
        //Debug.Log(GetCumulativeReward());
        if (GetCumulativeReward() < -8)
        {
            controller.CarDisable(this);
        }

       }

    public void ObjectCollided(Collider other)
    {
        AddReward(-12f);
        //Debug.LogWarning("Car " + tag + " - Collided with " + other.tag);
        if (other.tag == "Vehicle")
        {
            //Debug.Log("CarCollision");
            controller.CarCollision();
        }
        else if(other.tag == "Map")
        {
            controller.CarDisable(this);
        }
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
            return -1f;
        }
        else
        {
            return 0.5f;
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

}