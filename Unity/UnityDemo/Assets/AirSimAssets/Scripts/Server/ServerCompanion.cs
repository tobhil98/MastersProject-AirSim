using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirSimUnity
{

    public class ServerCompanion
    {
        static ServerCompanion()
        {
            InitDelegators();
        }

        private static void InitDelegators()
        {
            PInvokeWrapper.InitServerManager(
               // Marshal.GetFunctionPointerForDelegate(new Func<AirSimPose, bool, string, bool>(SetPose)),
               // Add functions that can be called from server to Unity
            );
        }

        // Add vehicle

        // Add pedestrian


        // Remove vehicle?


    }

}; // Namespace