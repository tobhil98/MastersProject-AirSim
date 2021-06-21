using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    float mainSpeed = 100.0f; //regular speed
    float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 1000.0f; //Maximum speed when holdin gshift
    float camSens = 0.25f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;

    enum CameraSetting
    {
        free = 0,
        vehicle,
        pedestrian
    }

    private CameraSetting cameraMode = 0;

    private int VehicleIndx = 0;
    private int PedestrianIndx = 0;

    private SmoothFollow smoothFollow;
    private void Start()
    {
        smoothFollow = transform.GetComponent<SmoothFollow>();
    }

    void Update()
    {

        lastMouse = Input.mousePosition - lastMouse;
        lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
        lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
        if (Input.GetMouseButton(1))
        {
            cameraMode = 0;
            smoothFollow.target = null;

            //Mouse  camera angle done.  
            transform.eulerAngles = lastMouse;
            //Keyboard commands
            float f = 0.0f;
            var p = GetBaseInput();
            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalRun += Time.deltaTime;
                p = p * totalRun * shiftAdd;
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            }
            else
            {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1, 1000);
                p = p * mainSpeed;
            }

            p = p * Time.deltaTime;
            if (Input.GetKey(KeyCode.Space))
            { //If player wants to move on X and Z axis only
                f = transform.position.y;
                transform.Translate(p);
                transform.position = new Vector3(transform.position.x, f, transform.position.z);
            }
            else
            {
                transform.Translate(p);
            }
        }
        UpdateCameraPointer();
        lastMouse = Input.mousePosition;
    }

    private Vector3 GetBaseInput() { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = Vector3.zero;
        if (Input.GetKey (KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0 , 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }

    private void UpdateCameraPointer()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cameraMode = CameraSetting.free;
            smoothFollow.target = null;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (AirSimUnity.AirSimServer.vehicleList.Count > 0)
            {
                cameraMode = CameraSetting.vehicle;
                if (AirSimUnity.AirSimServer.vehicleList.Count <= VehicleIndx)
                {
                    VehicleIndx = AirSimUnity.AirSimServer.vehicleList.Count - 1;
                }
                smoothFollow.target = AirSimUnity.AirSimServer.vehicleList[VehicleIndx];
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (AirSimUnity.AirSimServer.pedestrianList.Count > 0)
            {
                cameraMode = CameraSetting.pedestrian;
                if (AirSimUnity.AirSimServer.pedestrianList.Count <= PedestrianIndx)
                {
                    PedestrianIndx = AirSimUnity.AirSimServer.pedestrianList.Count - 1;
                }
                smoothFollow.target = AirSimUnity.AirSimServer.pedestrianList[PedestrianIndx].GetChild(0);
            }
        }



        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            if (cameraMode == CameraSetting.pedestrian)
            {
                PedestrianIndx = mod((PedestrianIndx - 1), AirSimUnity.AirSimServer.pedestrianList.Count);
                smoothFollow.target = AirSimUnity.AirSimServer.pedestrianList[PedestrianIndx].GetChild(0);
            }
            else if (cameraMode == CameraSetting.vehicle)
            {
                VehicleIndx = mod((VehicleIndx - 1), AirSimUnity.AirSimServer.vehicleList.Count);
                smoothFollow.target = AirSimUnity.AirSimServer.vehicleList[VehicleIndx];
            }

        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (cameraMode == CameraSetting.pedestrian)
            {
                PedestrianIndx = mod((PedestrianIndx + 1), AirSimUnity.AirSimServer.pedestrianList.Count);
                smoothFollow.target = AirSimUnity.AirSimServer.pedestrianList[PedestrianIndx].GetChild(0);

            }
            else if (cameraMode == CameraSetting.vehicle)
            {
                VehicleIndx = mod((VehicleIndx + 1), AirSimUnity.AirSimServer.vehicleList.Count);
                smoothFollow.target = AirSimUnity.AirSimServer.vehicleList[VehicleIndx];
            }
        }

    }
    private int mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }
}
