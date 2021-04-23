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

        private string pedestrianName;

        static PedestrianCompanion()
        {
            InitDelegators();
        }

        public static void DestroyPedestrian(string name)
        {
            Debug.LogWarning("Destroy pedestrian");
            var person = Pedestrians.Find(element => element.pedestrianName == name);
            //vehicle.VehicleInterface.DestroySelf();
            Pedestrians.Remove(person);
        }

        private PedestrianCompanion()//IVehicleInterface vehicleInterface)
        {
            //VehicleInterface = vehicleInterface;
            //basePortId = AirSimSettings.GetSettings().GetPortIDForVehicle(isDrone);
        }

        public bool StartPedestrianServer(string hostIP)
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
    }



}