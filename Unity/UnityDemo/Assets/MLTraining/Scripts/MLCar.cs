using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField] private Transform targetTransform;

    public override void OnActionReceived(ActionBuffers actions)
    {
        carControls.handbrake = actions.DiscreteActions[0]>0 ? true : false;
        carControls.throttle = actions.ContinuousActions[0];
        carControls.brake = actions.ContinuousActions[1];
        carControls.steering = actions.ContinuousActions[2];
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnEpisodeBegin()
    {
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.localPosition = startPos;
        transform.rotation = startRotation;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        ActionSegment<int> descreteActions = actionsOut.DiscreteActions;
        continousActions[0] = Input.GetAxis("Vertical");
        continousActions[1] = Input.GetAxis("Vertical");
        continousActions[2] = Input.GetAxis("Horizontal");
        descreteActions[0] = Input.GetAxis("Jump")>0 ? 1 : 0;
    }


    private void Start()
    {
        Debug.Log("Car start");
        carController = transform.GetComponent<AirSimCarController>();
        carControls.Reset();
        startPos = transform.localPosition;
        startRotation = transform.rotation;
        //Debug.LogWarning("A warning assigned to this transform!");

    }

    public void FixedUpdate()
    {
/*            if (resetVehicle)
        {
            resetVehicle = false;
            carData.Reset();
            carControls.Reset();
            rcData.Reset();
            DataManager.SetToUnity(poseFromAirLib.position, ref position);
            DataManager.SetToUnity(poseFromAirLib.orientation, ref rotation);
            transform.position = position;
            transform.rotation = rotation;
            currentPose = poseFromAirLib;
            steering = 0;
            throttle = 0;
            footBreak = 0;
            handBrake = 0;

            var rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.constraints = RigidbodyConstraints.None;
        }
        else
        {*/
            /*  if (isApiEnabled)
            {*/
                throttle = carControls.throttle;
                handBrake = carControls.handbrake ? 1 : 0;
                footBreak = carControls.brake;
                steering = carControls.steering;
            /*}
            else
            {

                steering = Input.GetAxis("Horizontal");
                throttle = Input.GetAxis("Vertical");
                handBrake = Input.GetAxis("Jump");
                footBreak = throttle;
            }
*/
            carController.Move(steering, throttle, footBreak, handBrake);
            carController.UpdateCarData(ref carData);
            carData.throttle = throttle;
            carData.brake = footBreak;
            carData.steering = steering;


        if(Vector3.Distance(transform.localPosition, targetTransform.localPosition) < 15)
        {
            SetReward(1f);
            EndEpisode();
        }




        }

    public void ObjectCollided(Collider other)
    {
        SetReward(-1f);
        EndEpisode();
    }

}

        //Car controls being set through client api calls
/*        public bool SetCarControls(CarControls controls)
        {
            DataManager.SetCarControls(controls, ref carControls);
            return true;
        }*/


        //Get the current car data to save in a text file along with the images taken while }
