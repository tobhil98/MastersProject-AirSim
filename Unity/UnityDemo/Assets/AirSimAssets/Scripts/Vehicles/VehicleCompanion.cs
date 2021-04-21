using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AirSimUnity.DroneStructs;
using AirSimUnity.CarStructs;
using UnityEngine;

namespace AirSimUnity {
    /*
     * An implementation of IAirSimInterface, facilitating calls from Unity to AirLib.
     * And also a bridge for the calls originating from AirLib into Unity.
     *
     * Unity client components should use an instance of this class to interact with AirLib.
     */

    internal class VehicleCompanion : IAirSimInterface {

        //All the vehicles that are created in this game.
        public static List<VehicleCompanion> Vehicles = new List<VehicleCompanion>();

        private static bool serverStarted = false;
        private static int basePortId;

        //An interface to interact with Unity vehicle component.
        private IVehicleInterface VehicleInterface;

        private string vehicleType;
        private string vehicleName;
        private readonly bool isDrone;


        static VehicleCompanion() {
            InitDelegators();
        }

        public static void DestroyVehicle(string name)
        {
            var vehicle = Vehicles.Find(element => element.vehicleName == name);
            vehicle.VehicleInterface.DestroySelf();
            Vehicles.Remove(vehicle);
        }

        private VehicleCompanion(IVehicleInterface vehicleInterface) {
            VehicleInterface = vehicleInterface;
            isDrone = vehicleInterface is Drone ? true : false;
            basePortId = AirSimSettings.GetSettings().GetPortIDForVehicle(isDrone);
        }

        public static VehicleCompanion GetVehicleCompanion(IVehicleInterface vehicleInterface, string vehicleName) {
            var companion = new VehicleCompanion(vehicleInterface);


            if (AirSimSettings.GetSettings().SimMode == "Car")
            {
                companion.vehicleType = "PhysXCar";
                companion.vehicleName = vehicleName;
            }

            else if (AirSimSettings.GetSettings().SimMode == "Multirotor")
                companion.vehicleType = "SimpleFlight";


            Vehicles.Add(companion);
            Debug.LogWarning("Number of cars: " + Vehicles.Count.ToString() + ". Added - " + companion.vehicleName);

            return companion;
        }

        public bool StartVehicleServer(string hostIP) {
            if (serverStarted == false){
                serverStarted = PInvokeWrapper.StartServer(vehicleType, AirSimSettings.GetSettings().SimMode, basePortId);
                Debug.LogWarning("Server started: " + serverStarted);
                return serverStarted;
            }
            return true;
        }

        public void StopVehicleServer() {
            if (serverStarted == true)
            {
                PInvokeWrapper.StopServer(vehicleType);
                Debug.LogWarning("Server halted");
                serverStarted = false;
            }
            else
            {
                Debug.LogWarning("Server already halted");
            }
        }

        public void InvokeTickInAirSim(float deltaSecond)
        {
            PInvokeWrapper.CallTick(deltaSecond);
        }

        public void InvokeCollisionDetectionInAirSim(CollisionInfo collisionInfo)
        {
            PInvokeWrapper.InvokeCollisionDetection(collisionInfo);
        }

        public KinemticState GetKinematicState() {
            return PInvokeWrapper.GetKinematicState(vehicleType);
        }

        public static DataRecorder.ImageData GetRecordingData() {
            return Vehicles[0].VehicleInterface.GetRecordingData();
        }

        public static DataCaptureScript GetCameraCaptureForRecording() {
            AirSimSettings.CamerasSettings recordCamSettings = AirSimSettings.GetSettings().Recording.Cameras[0];
            DataCaptureScript recordCam = Vehicles[0].VehicleInterface.GetCameraCapture(recordCamSettings.CameraName);
            return recordCam;
        }

        //Register the delegate functions to AirLib, based on IVehicleInterface
        private static void InitDelegators() {
            PInvokeWrapper.InitVehicleManager(
                Marshal.GetFunctionPointerForDelegate(new Func<AirSimPose, bool, string, bool>(SetPose)),
                Marshal.GetFunctionPointerForDelegate(new Func<string, AirSimPose>(GetPose)),
                Marshal.GetFunctionPointerForDelegate(new Func<string, CollisionInfo>(GetCollisionInfo)),
                Marshal.GetFunctionPointerForDelegate(new Func<string, AirSimRCData>(GetRCData)),
                Marshal.GetFunctionPointerForDelegate(new Func<ImageRequest, string, ImageResponse>(GetSimImages)),
                Marshal.GetFunctionPointerForDelegate(new Func<int, RotorInfo, string, bool>(SetRotorSpeed)),
                Marshal.GetFunctionPointerForDelegate(new Func<bool, string, bool>(SetEnableApi)),
                Marshal.GetFunctionPointerForDelegate(new Func<CarControls, string, bool>(SetCarApiControls)),
                Marshal.GetFunctionPointerForDelegate(new Func<string, CarState>(GetCarState)),
                Marshal.GetFunctionPointerForDelegate(new Func<string, string, CameraInfo>(GetCameraInfo)),
                Marshal.GetFunctionPointerForDelegate(new Func<string, AirSimPose, string, bool>(SetCameraPose)),
                Marshal.GetFunctionPointerForDelegate(new Func<string, float, string, bool>(SetCameraFoV)),
                Marshal.GetFunctionPointerForDelegate(new Func<string, string, float, string, bool>(SetDistortionParam)),
                Marshal.GetFunctionPointerForDelegate(new Func<string, string, bool>(GetDistortionParams)),
                Marshal.GetFunctionPointerForDelegate(new Func<string, int, bool, bool>(SetSegmentationObjectId)),
                Marshal.GetFunctionPointerForDelegate(new Func<string, int>(GetSegmentationObjectId)),
                Marshal.GetFunctionPointerForDelegate(new Func<string, string, string, int, bool>(PrintLogMessage)),
                Marshal.GetFunctionPointerForDelegate(new Func<string, UnityTransform>(GetTransformFromUnity)),
                Marshal.GetFunctionPointerForDelegate(new Func<string, bool>(Reset)),
                Marshal.GetFunctionPointerForDelegate(new Func<string, AirSimVector>(GetVelocity)),
                Marshal.GetFunctionPointerForDelegate(new Func<AirSimVector, AirSimVector, string, RayCastHitResult>(GetRayCastHit)),
                Marshal.GetFunctionPointerForDelegate(new Func<string, float, bool>(Pause))
            );
        }

        /*********************** Delegate functions to be registered with AirLib *****************************/

        private static bool SetPose(AirSimPose pose, bool ignoreCollision, string vehicleName) {
            var vehicle = Vehicles.Find(element => element.vehicleType == vehicleName);
            vehicle.VehicleInterface.SetPose(pose, ignoreCollision);
            return true;
        }

        private static AirSimPose GetPose(string vehicleName) {
            var vehicle = Vehicles.Find(element => element.vehicleType == vehicleName);
            return vehicle.VehicleInterface.GetPose();
        }

        private static CollisionInfo GetCollisionInfo(string vehicleName) {
            var vehicle = Vehicles.Find(element => element.vehicleType == vehicleName);
            return vehicle.VehicleInterface.GetCollisionInfo();
        }

        private static AirSimRCData GetRCData(string vehicleName) {
            var vehicle = Vehicles.Find(element => element.vehicleType == vehicleName);
            return vehicle.VehicleInterface.GetRCData();
        }

        private static ImageResponse GetSimImages(ImageRequest request, string vehicleName) {
            var vehicle = Vehicles.Find(element => element.vehicleName == vehicleName);
            if (vehicle != null)
                return vehicle.VehicleInterface.GetSimulationImages(request);
            return new ImageResponse(vehicleName);
        }

        private static UnityTransform GetTransformFromUnity(string vehicleName)
        {
            var vehicle = Vehicles.Find(element => element.vehicleType == vehicleName);
            return vehicle.VehicleInterface.GetTransform();
        }

        private static bool Reset(string vehicleName)
        {
            var vehicle = Vehicles.Find(element => element.vehicleType == vehicleName);
            vehicle.VehicleInterface.ResetVehicle();
            return true;
        }

        private static AirSimVector GetVelocity(string vehicleName)
        {
            var vehicle = Vehicles.Find(element => element.vehicleType == vehicleName);
            return vehicle.VehicleInterface.GetVelocity();
        }

        private static RayCastHitResult GetRayCastHit(AirSimVector start, AirSimVector end, string vehicleName)
        {
            var vehicle = Vehicles.Find(element => element.vehicleType == vehicleName);
            return vehicle.VehicleInterface.GetRayCastHit(start, end);
        }

        private static bool SetRotorSpeed(int rotorIndex, RotorInfo rotorInfo, string vehicleName) {
            var vehicle = Vehicles.Find(element => element.vehicleType == vehicleName);
            return vehicle.VehicleInterface.SetRotorSpeed(rotorIndex, rotorInfo);
        }

        private static bool SetEnableApi(bool enableApi, string vehicleName) {
            var vehicle = Vehicles.Find(element => element.vehicleName == vehicleName);
            if(vehicle != null)
                return vehicle.VehicleInterface.SetEnableApi(enableApi);
            return false;
        }

        private static bool SetCarApiControls(CarControls controls, string vehicleName) {
            var vehicle = Vehicles.Find(element => element.vehicleName == vehicleName);
            if(vehicle != null)
                return vehicle.VehicleInterface.SetCarControls(controls);
            return false;
        }

        private static CarState GetCarState(string vehicleName) {
            var vehicle = Vehicles.Find(element => element.vehicleType == vehicleName);
            return vehicle.VehicleInterface.GetCarState();
        }

        private static CameraInfo GetCameraInfo(string cameraName, string vehicleName) {
            var vehicle = Vehicles.Find(element => element.vehicleType == vehicleName);
            return vehicle.VehicleInterface.GetCameraInfo(cameraName);
        }

        private static bool SetCameraPose(string cameraName, AirSimPose pose, string vehicleName) {
            var vehicle = Vehicles.Find(element => element.vehicleType == vehicleName);
            return vehicle.VehicleInterface.SetCameraPose(cameraName, pose);
        }

        private static bool SetCameraFoV(string cameraName, float fov_degrees, string vehicleName) {
            var vehicle = Vehicles.Find(element => element.vehicleType == vehicleName);
            return vehicle.VehicleInterface.SetCameraFoV(cameraName, fov_degrees);
        }

        private static bool SetDistortionParam(string cameraName, string paramName, float value, string vehicleName) {
            var vehicle = Vehicles.Find(element => element.vehicleType == vehicleName);
            return vehicle.VehicleInterface.SetDistortionParam(cameraName, paramName, value);
        }

        private static bool GetDistortionParams(string cameraName, string vehicleName)
        {
            var vehicle = Vehicles.Find(element => element.vehicleType == vehicleName);
            return vehicle.VehicleInterface.GetDistortionParams(cameraName);
        }

        private static bool PrintLogMessage(string message, string messageParams, string vehicleName, int severity) {
            if(severity > 0)
            {
                Debug.LogWarning("PrintLogMessage " + severity + ": " + message + " - " + messageParams);
            }
            return true;
            //var vehicle = Vehicles.Find(element => element.vehicleType == vehicleName);
            //return vehicle.VehicleInterface.PrintLogMessage(message, messageParams, vehicleName, severity);
        }

        private static bool SetSegmentationObjectId(string objectName, int objectId, bool isNameRegex) {
            return Vehicle.SetSegmentationObjectId(objectName, objectId, isNameRegex);
        }

        private static int GetSegmentationObjectId(string objectName) {
            return Vehicle.GetSegmentationObjectId(objectName);
        }

        private static bool Pause(string vehicleName, float timeScale)
        {
            var vehicle = Vehicles.Find(element => element.vehicleType == vehicleName);
            return vehicle.VehicleInterface.Pause(timeScale);
        }
    }
}
