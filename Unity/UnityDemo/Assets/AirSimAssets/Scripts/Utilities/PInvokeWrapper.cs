using System;
using System.Runtime.InteropServices;

namespace AirSimUnity {
    /*
     * class for all the PInvoke methods.
     */

    public static class PInvokeWrapper {
        private const string DLL_NAME = "AirsimWrapper";

        // Delegates initializer. All the delegate methods are registered through this PInvoke call
        [DllImport(DLL_NAME)]
        public static extern void InitVehicleManager(IntPtr SetPose, IntPtr GetPose, IntPtr GetCollisionInfo, IntPtr GetRCData,
            IntPtr GetSimImages, IntPtr SetRotorSpeed, IntPtr SetEnableApi, IntPtr SetCarApiControls, IntPtr GetCarState,
            IntPtr GetCameraInfo, IntPtr SetCameraPose, IntPtr SetCameraFoV, IntPtr SetDistortionParam, IntPtr GetDistortionParams,
            IntPtr SetSegmentationObjectId, IntPtr GetSegmentationObjectId, IntPtr PrintLogMessage, IntPtr GetTransformFromUnity, 
            IntPtr Reset, IntPtr GetVelocity, IntPtr GetRayCastHit, IntPtr Pause);

        

        [DllImport(DLL_NAME)]
        public static extern KinemticState GetKinematicState(string vehicleName);

        [DllImport(DLL_NAME)]
        public static extern void StartDroneServer(string ip, int port, string vehicleId);

        [DllImport(DLL_NAME)]
        public static extern void StopDroneServer(string vehicleName);

        [DllImport(DLL_NAME)]
        public static extern bool StartServer(string vehicleName, string simModeName, int portNumber);

        [DllImport(DLL_NAME)]
        public static extern void StopServer(string vehicleName);

        [DllImport(DLL_NAME)]
        public static extern void CallTick(float deltaSeconds);

        [DllImport(DLL_NAME)]
        public static extern void InvokeCollisionDetection(CollisionInfo collisionInfo);
        
        [DllImport(DLL_NAME)]
        public static extern void StoreImage(string vehicle_name, string camera_name, ImageResponse image);


        // SERVER //
        [DllImport(DLL_NAME)]
        public static extern void InitServerManager(IntPtr PrintTest, IntPtr AddVehicle, IntPtr AddPedestrianFunc, IntPtr RemoveVehicle, IntPtr RemovePedestrian);

        [DllImport(DLL_NAME)]
        public static extern bool StartMainServer(int portNumber);
        [DllImport(DLL_NAME)]
        public static extern void StopMainServer();

        // PEDESTRIANS // 
        [DllImport(DLL_NAME)]
        public static extern void InitPedestrianManager(IntPtr SetPose, IntPtr GetPose, IntPtr Reset, IntPtr SetEnableApi, IntPtr SetPedestrianApiControls);

        [DllImport(DLL_NAME)]
        public static extern bool StartPedestrianServer(int portNumber);
        [DllImport(DLL_NAME)]
        public static extern void StopPedestrianServer();
    }
}
