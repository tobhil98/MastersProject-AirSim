using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using AirSimUnity.DroneStructs;
using System.Threading;
using UnityEditor;

namespace AirSimUnity
{
    public class AirSimServer : MonoBehaviour
    {
        private const string DRONE_MODE = "Multirotor";

        // Start is called before the first frame update
        void Start()
        {
            Debug.LogError("Script called");
            string simMode = AirSimSettings.GetSettings().SimMode;
            int basePortId = AirSimSettings.GetSettings().GetPortIDForVehicle(simMode == DRONE_MODE);
            Debug.LogError("Check here: " + simMode + ", " + basePortId);
            bool isServerStarted = PInvokeWrapper.StartServer(simMode, basePortId);
            Debug.LogError("Check again: " + isServerStarted);
            return;
            if (isServerStarted == false)
            {
#if UNITY_EDITOR
                EditorUtility.DisplayDialog("Problem in starting AirSim server!!!", "Please check logs for more information.", "Exit");
                //EditorApplication.Exit(1);
#else
                Application.Quit();
#endif
            }
        }

        protected void OnApplicationQuit()
        {
            PInvokeWrapper.StopServer();
        }
    }
}