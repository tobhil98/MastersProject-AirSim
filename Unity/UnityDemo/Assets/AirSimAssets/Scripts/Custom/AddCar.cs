using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCar : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform car;

    public void Pressed()
    {
        Debug.LogWarning("Adding new car");
        Instantiate(car, new Vector3(-250, 2, 50), Quaternion.identity);
    }

}
