using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
               Marshal.GetFunctionPointerForDelegate(new Func<string, bool>(PrintTest)),
               Marshal.GetFunctionPointerForDelegate(new Func<string, string, bool>(AddVehicle)),
               Marshal.GetFunctionPointerForDelegate(new Func<string, string, bool>(RemoveVehicle))
               // Add functions that can be called from server to Unity
            );
        }

        private static bool PrintTest(string message)
        {
            Debug.LogError("Custom message:" + message);
            return true;
        }

        // Add vehicle
        private static bool AddVehicle(string vehicle_name, string vehicle_type) // Take in init pose and path?
        {
            Debug.LogError("Attempting to add car: " + vehicle_name + " - " + vehicle_type);
            AddCar.GetInstance().SpawnVehicle(vehicle_name, new Vector3(-250, 2, 50), Quaternion.identity);
            return true;
        }

        // Add pedestrian


        // Remove vehicle
        private static bool RemoveVehicle(string vehicle_name, string vehicle_type) // Take in init pose and path?
        {
            Debug.LogError("Attempting to remove car: " + vehicle_name + " - " + vehicle_type);
            VehicleCompanion.DestroyVehicle(vehicle_name);
            return true;
        }

    }

}; // Namespace