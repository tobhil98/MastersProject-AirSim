using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetHandler : MonoBehaviour
{
    // TODOME: Create something to set up scene
    public VehicleInput[] vehicles;

    public Transform randomPedestrian;
    public RuntimeAnimatorController pedestrianAnimation;

    private static AssetHandler instance;

    AssetHandler()
    {
        instance = this;
    }

    public static AssetHandler getInstance()
    {
        return instance;
    }


    public Transform getPedestrian()
    {
        return randomPedestrian;
    }

    public string getVehicle()
    {
        return vehicles[0].name;
    }

    [Serializable]
    public struct VehicleInput
    {
        public string name;
        public Transform vehicle;
    };
}
