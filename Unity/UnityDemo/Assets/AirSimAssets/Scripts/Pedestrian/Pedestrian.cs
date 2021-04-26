using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityStandardAssets.Characters.ThirdPerson;


namespace AirSimUnity
{
    public class Pedestrian : MonoBehaviour
    {
        private PedestrianCompanion pedestrianInterface;
        private bool isServerStarted = false;
        private bool destroySelf_ = false;
        private bool isApiEnabled = false;


        public struct PedestrianController
        {
            public float speed;
            public float steeringRotation;
            public PedestrianController(float speed, float steeringRotation)
            {
                this.speed = speed;
                this.steeringRotation = steeringRotation;
            }
        };
        PedestrianController pc;

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
            pc = new PedestrianController(0, 0);
        }

        private void FixedUpdate()
        {
            if (destroySelf_) Destroy(transform.parent.gameObject);

            if (isServerStarted)
            {
                if (isApiEnabled)
                {
                    steering = pc.steeringRotation;
                    speed = pc.speed;
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