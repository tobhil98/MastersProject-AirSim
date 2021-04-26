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
    }



}