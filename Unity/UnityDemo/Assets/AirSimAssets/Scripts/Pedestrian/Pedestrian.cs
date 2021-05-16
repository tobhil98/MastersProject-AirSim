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
        public List<DataCaptureScript> captureCameras = new List<DataCaptureScript>();
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

        private bool isCapturingImages = false;
        private ImageRequest imageRequest;
        private ImageResponse imageResponse;


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
            if (destroySelf_)
            {
                AirSimServer.pedestrianList.Remove(transform.parent);
                Destroy(transform.parent.gameObject);
            }

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

        private int count = 0;
        private void LateUpdate()
        {

            if (count > 5)
            {
                count = 0;
                foreach (var p in captureCameras)
                {
                    string camera = p.GetCameraName();
                    var imageRequest = new ImageRequest(camera, ImageType.Scene, false, false);

                    imageResponse = p.GetImageBasedOnRequest(imageRequest);
                    PInvokeWrapper.StorePedestrianImage(pedestrian_name, camera, imageResponse);
                }
            }
            count++;
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

        public bool SetPedestrianControls(PedestrianControls controls)
        {
            DataManager.SetPedestrianControls(controls, ref pedestrianControls);
            return true;
        }
        public void DestroySelf()
        {
            destroySelf_ = true;
        }

        private void OnApplicationQuit()
        {
            pedestrianInterface.StopPedestrianServer();
        }

        private void InitialisePedestrian()
        {
            transform.GetComponent<Animator>().runtimeAnimatorController = AssetHandler.getInstance().pedestrianAnimation;
            SetUpCameras();
        }

        private void SetUpCameras()
        {
            GameObject camerasParent = transform.Find("CaptureCameras").gameObject;
            //GameObject camerasParent = GameObject.FindGameObjectWithTag("CaptureCameras"); 

            if (!camerasParent)
            {
                Debug.LogWarning("No Cameras found in the scene to capture data");
                return;
            }

            for (int i = 0; i < camerasParent.transform.childCount; i++)
            {
                DataCaptureScript camCapture = camerasParent.transform.GetChild(i).GetComponent<DataCaptureScript>();
                captureCameras.Add(camCapture);
                camCapture.SetUpCamera(camCapture.GetCameraName(), false);
            }
        }
    };

};