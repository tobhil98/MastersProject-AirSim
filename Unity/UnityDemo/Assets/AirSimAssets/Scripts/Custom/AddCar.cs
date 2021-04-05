using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AirSimUnity.CarStructs;


namespace AirSimUnity
{
    public class AddCar : MonoBehaviour
    {
        // Start is called before the first frame update

        public Transform car;


        private static int count = 1;
        public void Pressed()
        {
            Debug.LogWarning("Adding new car");
            var obj = Instantiate(car, new Vector3(-250, 2, 50), Quaternion.identity);
            obj.GetComponent<Car>().vehicleName = "Car" + (count++).ToString();
        }

    }

}