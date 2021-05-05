using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using AirSimUnity.PedestrianStructs;

namespace AirSimUnity
{
    public class PedestrianCompanion
    {
        public static List<PedestrianCompanion> Pedestrians = new List<PedestrianCompanion>();

        public static bool serverStarted = false;
        private static int basePortId = 41452;

        public string pedestrianName;

        private Pedestrian pedestrianPtr;

        static PedestrianCompanion()
        {
            InitDelegators();
        }

        public static void DestroyPedestrian(string name)
        {
            Debug.LogWarning("Destroy pedestrian");
            var person = Pedestrians.Find(element => element.pedestrianName == name);
            person.pedestrianPtr.DestroySelf();
            Pedestrians.Remove(person);
        }

        public static PedestrianCompanion GetPedestrianCompanion(Pedestrian ped, string pedestrianName)
        {
            var companion = new PedestrianCompanion();
            companion.pedestrianName = pedestrianName;
            companion.pedestrianPtr = ped;

            Pedestrians.Add(companion);
            Debug.LogWarning("Number of pedestrians: " + Pedestrians.Count.ToString() + ". Added - " + companion.pedestrianName);

            return companion;
        }

        private PedestrianCompanion()//IVehicleInterface vehicleInterface)
        {
            //VehicleInterface = vehicleInterface;
            //basePortId = AirSimSettings.GetSettings().GetPortIDForVehicle(isDrone);
        }

        public bool StartPedestrianServer(int hostIP)
        {
            if (serverStarted == false)
            {
                serverStarted = PInvokeWrapper.StartPedestrianServer(basePortId);
                Debug.Log("Pedestrian server startedv on port: " + basePortId);
                return serverStarted;
            }
            return true;
        }
        public void StopPedestrianServer()
        {
            if (serverStarted == true)
            {
                PInvokeWrapper.StopPedestrianServer();
                Debug.Log("Pedestrian server halted");
                serverStarted = false;
            }
        }
        private static void InitDelegators()
        {
            PInvokeWrapper.InitPedestrianManager(
                Marshal.GetFunctionPointerForDelegate(new Func<AirSimPose, bool, string, bool>(SetPose)),
                Marshal.GetFunctionPointerForDelegate(new Func<string, AirSimPose>(GetPose)),
                Marshal.GetFunctionPointerForDelegate(new Func<string, bool>(Reset)),
                Marshal.GetFunctionPointerForDelegate(new Func<bool, string, bool>(SetEnableApi)),
                Marshal.GetFunctionPointerForDelegate(new Func<PedestrianControls, string, bool>(SetPedestrianApiControls)),
                Marshal.GetFunctionPointerForDelegate(new Func<string, ServerUtils.StringArray>(GetPedestrianCameras))
            );
        }

        private static bool SetPose(AirSimPose pose, bool ignoreCollision, string pedestrianName)
        {
            var pedestrian = Pedestrians.Find(element => element.pedestrianName == pedestrianName);
            pedestrian.pedestrianPtr.SetPose(pose, ignoreCollision);
            return true;
        }

        private static AirSimPose GetPose(string pedestrianName)
        {
            var pedestrian = Pedestrians.Find(element => element.pedestrianName == pedestrianName);
            return pedestrian.pedestrianPtr.GetPose();
        }

        private static bool Reset(string pedestrianName)
        {
            /*  var pedestrian = Pedestrians.Find(element => element.pedestrianName == pedestrianName);
              pedestrian.pedestrianPtr.ResetPedestrian();*/
            Debug.Log("Reset called on pedestrian " + pedestrianName);
            return true;
        }

        private static bool SetEnableApi(bool enableApi, string pedestrianName)
        {
            if (pedestrianName == "")
            {
                foreach (var p in Pedestrians)
                {
                    if (p != null)
                    {
                        p.pedestrianPtr.SetEnableApi(enableApi);
                    }
                    else
                    {
                        Debug.LogError("Should not happen - Check why this happend");
                    }
                }
                return true;
            }

            var pedestrian = Pedestrians.Find(element => element.pedestrianName == pedestrianName);
            if (pedestrian != null)
                return pedestrian.pedestrianPtr.SetEnableApi(enableApi);
            return false;
        }

        private static bool SetPedestrianApiControls(PedestrianControls controls, string pedestrianName)
        {
            if (pedestrianName == "")
            {
                foreach(var p in Pedestrians)
                {
                    if(p != null)
                    {
                        p.pedestrianPtr.SetPedestrianControls(controls);
                    }
                    else
                    {
                        Debug.LogError("Should not happen - Check why this happend");
                    }

                }
                return true;
            }

            var pedestrian = Pedestrians.Find(element => element.pedestrianName == pedestrianName);
            if (pedestrian != null)
                return pedestrian.pedestrianPtr.SetPedestrianControls(controls);
            return false;
        }

        private static ServerUtils.StringArray GetPedestrianCameras(string pedestrianName)
        {
            var pedestrian = Pedestrians.Find(element => element.pedestrianName == pedestrianName);
            ServerUtils.StringArray array = new ServerUtils.StringArray();
            List<string> lst = new List<string>();
            foreach (var c in pedestrian.pedestrianPtr.captureCameras)
            {
                lst.Add(c.cameraName);
            }
            DataManager.ConvertToStringArray(lst, ref array);
            return array;
        }
    }

}