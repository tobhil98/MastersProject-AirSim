using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleCollider : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform vehicle;


    private void OnTriggerEnter(Collider other)
    {
        vehicle.GetComponent<MLCar>().ObjectCollided(other);
    }
}
