using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetHandler : MonoBehaviour
{
    // TODOME: Create something to set up scene

    public Transform vehicle;
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

    public Transform getVehicle()
    {
        return vehicle;
    }

}
