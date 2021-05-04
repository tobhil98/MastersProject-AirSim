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
               Marshal.GetFunctionPointerForDelegate(new Func<string, bool>(AddPedestrianFunc)),
               Marshal.GetFunctionPointerForDelegate(new Func<string, string, bool>(RemoveVehicle)),
               Marshal.GetFunctionPointerForDelegate(new Func<string, bool>(RemovePedestrian)),
               Marshal.GetFunctionPointerForDelegate(new Func<ServerUtils.VehicleTypes>(GetVehicleTypes))
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
            if (vehicle_type == "")
            {
                vehicle_type = AssetHandler.getInstance().getVehicle();
            }
            AddCar.GetInstance().SpawnVehicle(vehicle_name, vehicle_type, new Vector3(-250, 2, 50), Quaternion.identity);
            return true;
        }

        // Add pedestrian
        private static bool AddPedestrianFunc(string vehicle_name) // Take in init pose and path?
        {
            Debug.LogError("Attempting to add pedestrian: " + vehicle_name);
            AddPedestrain.GetInstance().SpawnPedestrian(vehicle_name, new Vector3(-247, 2, 50), Quaternion.identity);
            return true;
        }

        // Remove vehicle
        private static bool RemoveVehicle(string vehicle_name, string vehicle_type) // Take in init pose and path?
        {
            VehicleCompanion.DestroyVehicle(vehicle_name);
            return true;
        }

        // Remove pedestrian
        private static bool RemovePedestrian(string vehicle_name) // Take in init pose and path?
        {
            PedestrianCompanion.DestroyPedestrian(vehicle_name);
            return true;
        }

        private static ServerUtils.VehicleTypes GetVehicleTypes()
        {
            ServerUtils.VehicleTypes c = new ServerUtils.VehicleTypes();
            List<string> lst = new List<string>();
            foreach(var i in AssetHandler.getInstance().vehicles)
            {
                lst.Add(i.name);
            }
            DataManager.ConvertToVehicleTypes(lst, ref c);
            return c;
        }

    }

}; // Namespace