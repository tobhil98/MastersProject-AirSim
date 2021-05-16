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
               Marshal.GetFunctionPointerForDelegate(new Func<ServerUtils.StringArray>(GetVehicleTypes)),
               Marshal.GetFunctionPointerForDelegate(new Func<ServerUtils.StringArray>(GetAllVehiclesList)),
               Marshal.GetFunctionPointerForDelegate(new Func<ServerUtils.StringArray>(GetAllPedestriansList))
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

        private static ServerUtils.StringArray GetVehicleTypes()
        {
            ServerUtils.StringArray array = new ServerUtils.StringArray();
            List<string> lst = new List<string>();
            foreach(var i in AssetHandler.getInstance().vehicles)
            {
                lst.Add(i.name);
            }
            DataManager.ConvertToStringArray(lst, ref array);
            return array;
        }

        private static ServerUtils.StringArray GetAllVehiclesList()
        {
            ServerUtils.StringArray array = new ServerUtils.StringArray();
            List<string> lst = new List<string>();
            foreach (var i in VehicleCompanion.Vehicles)
            {
                lst.Add(i.vehicleName);
            }
            DataManager.ConvertToStringArray(lst, ref array);
            return array;
        }

        private static ServerUtils.StringArray GetAllPedestriansList()
        {
            ServerUtils.StringArray array = new ServerUtils.StringArray();
            List<string> lst = new List<string>();
            foreach (var i in PedestrianCompanion.Pedestrians)
            {
                lst.Add(i.pedestrianName);
            }
            DataManager.ConvertToStringArray(lst, ref array);
            return array;
        }


    }

}; // Namespace