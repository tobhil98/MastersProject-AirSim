using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace AirSimUnity
{

    public class AirSimServer : MonoBehaviour
    {
        public string IP;
        public int Port = -1;
        // Start is called before the first frame update
        private ServerCompanion comp;

        public static List<Transform> vehicleList;
        public static List<Transform> pedestrianList;

    /*    AirSimServer()
        {
        }*/

        void Start()
        {
            vehicleList = new List<Transform>();
            pedestrianList = new List<Transform>();

            comp = new ServerCompanion();
            var settings = AirSimSettings.GetSettings();
                        
            if (IP == "")
                IP = settings.LocalHostIP;
            
            
            if (Port == -1)
                Port = settings.GetPort(AirSimSettings.AgentType.Server);

                // Start server
            bool status = PInvokeWrapper.StartMainServer(Port);
            Debug.LogWarning("Main server started on port " + Port.ToString());
            if(!status)
            {
#if UNITY_EDITOR
                EditorUtility.DisplayDialog("AirSim main failed to start on port " + Port.ToString() + "!!!", "Please check logs for more information.", "Exit");
                EditorApplication.Exit(1);
#else
                Application.Quit();
#endif
            }

        }

        protected void OnApplicationQuit()
        {
            PInvokeWrapper.StopMainServer();
            Debug.LogWarning("Main server stopped");
            if(PedestrianCompanion.serverStarted)
            {
                PInvokeWrapper.StopPedestrianServer();
                PedestrianCompanion.serverStarted = false;
            }
            if(VehicleCompanion.serverStarted)
            {
                PInvokeWrapper.StopServer("");
                PedestrianCompanion.serverStarted = false;
            }
        }
    }

}; // Namspace