using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AirSimUnity
{
    public class PedestrianCompanion
    {
        public static List<PedestrianCompanion> Pedestrians = new List<PedestrianCompanion>();

        private static bool serverStarted = false;
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
                Debug.LogWarning("Pedestrian server started: " + serverStarted);
                return serverStarted;
            }
            return true;
        }
        public void StopPedestrianServer()
        {
            if (serverStarted == true)
            {
                PInvokeWrapper.StopPedestrianServer();
                Debug.LogWarning("Pedestrian server halted");
                serverStarted = false;
            }
            else
            {
                Debug.LogWarning("Pedestrian server already halted");
            }
        }
        private static void InitDelegators()
        {
            PInvokeWrapper.InitPedestrianManager(
            //Marshal.GetFunctionPointerForDelegate(new Func<AirSimPose, bool, string, bool>(SetPose)),
            );
        }
        /*
                public static extern void InitVehicleManager(IntPtr SetPose, IntPtr GetPose, IntPtr GetCollisionInfo, IntPtr GetRCData,
            IntPtr GetSimImages, IntPtr SetRotorSpeed, IntPtr SetEnableApi, IntPtr SetCarApiControls, IntPtr GetCarState,
            IntPtr GetCameraInfo, IntPtr SetCameraPose, IntPtr SetCameraFoV, IntPtr SetDistortionParam, IntPtr GetDistortionParams,
            IntPtr SetSegmentationObjectId, IntPtr GetSegmentationObjectId, IntPtr PrintLogMessage, IntPtr GetTransformFromUnity,
            IntPtr Reset, IntPtr GetVelocity, IntPtr GetRayCastHit, IntPtr Pause);*/


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
            var pedestrian = Pedestrians.Find(element => element.pedestrianName == pedestrianName);
            pedestrian.pedestrianPtr.ResetPedestrian();
            return true;
        }

        private static bool SetEnableApi(bool enableApi, string pedestrianName)
        {
            var pedestrian = Pedestrians.Find(element => element.pedestrianName == pedestrianName);
            if (pedestrian != null)
                return pedestrian.pedestrianPtr.SetEnableApi(enableApi);
            return false;
        }


    }



}