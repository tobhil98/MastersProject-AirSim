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

    private CarControls carControls;
    private CarState carState;
    private CarData carData;


    private float steering, throttle, footBreak, handBrake;

    private bool destroySelf_ = false;

    private Vector3 startPos;
    private Quaternion startRotation;

    [SerializeField] public Transform targetTransform;
    [SerializeField] private MLController controller;

    private float optimalDistance;
    private bool moved = false;
    private int stuckCount = 0;

    public override void OnActionReceived(ActionBuffers actions)
    {
        //carControls.handbrake = actions.DiscreteActions[0]>0 ? true : false;
        carControls.throttle = actions.ContinuousActions[0];
        carControls.brake = actions.ContinuousActions[0];
        carControls.steering = actions.ContinuousActions[1];
    }
/*    public override void OnActionReceived(float[] vectorAction)
    {
        carControls.throttle = vectorAction[0];
        carControls.brake = vectorAction[0];
        carControls.steering = vectorAction[1];
    }*/

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 v = targetTransform.localPosition - transform.localPosition;
        float directionDot = (float)System.Math.Tanh(Vector3.Angle(transform.forward.normalized, v)/180f);
        sensor.AddObservation(directionDot);
        float turn = AngleDir(transform.forward, v, transform.up);
        sensor.AddObservation(turn);
        //Debug.Log("Observation: " + directionDot + " - " + turn);
    }

    public override void OnEpisodeBegin()
    {
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.localPosition = startPos;
        transform.rotation = startRotation;

        transform.localPosition += new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        transform.Rotate(0, Random.Range(-30, 100), 0, Space.Self);
        targetTransform.localPosition = new Vector3(Random.Range(-40, 40), targetTransform.localPosition.y, Random.Range(20, 40));

        optimalDistance = Vector3.Distance(transform.localPosition, targetTransform.localPosition);

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
        carController = transform.GetComponent<AirSimCarController>();
        carControls.Reset();
        startPos = transform.localPosition;
        startRotation = transform.rotation;
        //Debug.LogWarning("A warning assigned to this transform!");

    }

    public void FixedUpdate()
    {
        throttle = carControls.throttle;
        handBrake = 0; //carControls.handbrake ? 1 : 0;
        footBreak = carControls.brake;
        steering = carControls.steering;

        carController.Move(steering, throttle, footBreak, handBrake);
        carController.UpdateCarData(ref carData);
        carData.throttle = throttle;
        carData.brake = footBreak;
        carData.steering = steering;

        /*Debug.LogWarning("HandBreak: " + handBrake);
        Debug.LogWarning("Steering: " + steering);*/
        
        if (Vector3.Distance(transform.localPosition, targetTransform.localPosition) < optimalDistance-1)
        {
            moved = true;
            stuckCount = 0;
            AddReward(0.05f);
            optimalDistance = Vector3.Distance(transform.localPosition, targetTransform.localPosition);
            
            Vector3 v = Vector3.Scale((targetTransform.position - transform.position), new Vector3(1, 0, 1)).normalized;
            float a = Vector3.Angle(Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized, v);

            if (a < 10)
            {
                if (throttle > 0.2)
                {
                    AddReward((10 - a) / 200);
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

        if (StepCount > 500 && moved == false)
        {
            //PrefabUtility.RevertObjectOverride(gameObject);
            /*            Destroy(gameObject);
                        controller.resetMap();*/
            Debug.Log("StuckCount " + ++stuckCount);
            EpisodeInterrupted();
        }

        if (Vector3.Distance(transform.localPosition, targetTransform.localPosition) < 13)
        {
            AddReward(10f);
            EndEpisode();
        }


       }

    public void ObjectCollided(Collider other)
    {
        AddReward(-7f);
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

    private void OnDisable()
    {
        Debug.LogError("Car dissabled");
    }

}