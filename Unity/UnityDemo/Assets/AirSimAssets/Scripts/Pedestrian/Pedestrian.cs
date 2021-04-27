using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityStandardAssets.Characters.ThirdPerson;
using AirSimUnity.PedestrianStructs;


namespace AirSimUnity
{
    public class Pedestrian : MonoBehaviour
    {
        private PedestrianCompanion pedestrianInterface;
        private bool isServerStarted = false;
        private bool destroySelf_ = false;
        private bool resetPedestrian_ = false;
        private bool isApiEnabled = false;

        private PedestrianControls pedestrianControls;
        private PedestrianState pedestrianState;
        private PedestrianData pedestrianData;

        protected AirSimPose poseFromAirLib;
        protected AirSimPose currentPose;

        public string pedestrian_name;
        private float steering, speed;


        public void Start()
        {
            pedestrian_name = transform.GetComponentInParent<PedestrianOverhead>().name;

            InitialisePedestrian();

            pedestrianInterface = PedestrianCompanion.GetPedestrianCompanion(this, pedestrian_name);
            isServerStarted = pedestrianInterface.StartPedestrianServer(AirSimSettings.GetSettings().GetPort(AirSimSettings.AgentType.Pedestrian));

            if (isServerStarted == false)
            {
#if UNITY_EDITOR
                EditorUtility.DisplayDialog("Problem in starting AirSim server!!!", "Please check logs for more information.", "Exit");
                EditorApplication.Exit(1);
#else
                Application.Quit();
#endif
            }

            //AirSimGlobal.Instance.Weather.AttachToVehicle(this);

            //count = UnityEngine.Random.Range(0, 10);
            pedestrianControls = new PedestrianControls(0, 0);
        }

        private void FixedUpdate()
        {
            if (destroySelf_) Destroy(transform.parent.gameObject);

            if (isServerStarted)
            {
                DataManager.SetToAirSim(transform.position, ref currentPose.position);
                DataManager.SetToAirSim(transform.rotation, ref currentPose.orientation);

                if (isApiEnabled)
                {
                    steering = pedestrianControls.steering;
                    speed = pedestrianControls.speed;
                }
                else
                {

                    steering = Input.GetAxis("Horizontal");
                    speed = Input.GetAxis("Vertical") * 0.5f ;
                    //jump= Input.GetAxis("Jump");
                    //footBreak = throttle;
                }

                speed = speed > 0 ? speed : 0;

                var pedestrianController = transform.GetComponent<ThirdPersonCharacter>();

                //Vector3.right * steering 

                //Debug.Log("Pos: " + transform.rotation.eulerAngles.normalized + " - Angle: " + angle);
                var pedestrianForward = Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized;
                var move = speed * pedestrianForward + steering * transform.right;


               /* var angle = Vector3.Scale((transform.rotation + Vector3.right * (steering+0.2f) ), (Vector3.one * speed));

                //var t = transform.rotation + angle;
                Debug.Log("Pos: " + transform.rotation.eulerAngles.normalized + " - Angle: " + angle);*/
                pedestrianController.Move(move, false, false);
/*                carController.UpdateCarData(ref carData);
                carData.throttle = throttle;
                carData.brake = footBreak;
                carData.steering = steering;*/
            }

        }


        public bool SetEnableApi(bool enableApi)
        {
            Debug.Log("Updated pedestrian api - " + enableApi);
            isApiEnabled = enableApi;
            return true;
        }

        public void ResetPedestrian()
        {
            resetPedestrian_ = true;
        }

        public AirSimVector GetVelocity()
        {
            var rigidBody = GetComponent<Rigidbody>();
            return new AirSimVector(rigidBody.velocity.x, rigidBody.velocity.y, rigidBody.velocity.z);
        }

        public bool SetPose(AirSimPose pose, bool ignore_collision)
        {
            poseFromAirLib = pose;
            return true;
        }

        public AirSimPose GetPose()
        {
            return currentPose;
        }

        public bool SetCarControls(PedestrianControls controls)
        {
            DataManager.SetPedestrianControls(controls, ref pedestrianControls);
            return true;
        }

        private void OnApplicationQuit()
        {
            pedestrianInterface.StopPedestrianServer();
        }

        private void InitialisePedestrian()
        {
            transform.GetComponent<Animator>().runtimeAnimatorController = AssetHandler.getInstance().pedestrianAnimation;
        }

        public void DestroySelf()
        {
            destroySelf_ = true;
        }
    };

};