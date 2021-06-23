#include "PInvokeWrapper.h"

//Function pointers to hold the addresses of the functions that are defined in Unity
bool(*SetPose)(AirSimPose pose, bool ignoreCollision, const char* vehicleName);
AirSimPose(*GetPose)(const char* vehicleName);
AirSimCollisionInfo(*GetCollisionInfo)(const char* vehicleName);
AirSimRCData(*GetRCData)(const char* vehicleName);
AirSimImageResponse(*GetSimImages)(AirSimImageRequest request, const char* vehicleName);
bool(*SetRotorSpeed)(int rotorIndex, RotorInfo rotorInfo, const char* vehicleName);
bool(*SetEnableApi)(bool enableApi, const char* vehicleName);
bool(*SetCarApiControls)(msr::airlib::CarControls controls, const char* vehicleName);
AirSimCarState(*GetCarState)(const char* vehicleName);
AirSimCameraInfo(*GetCameraInfo)(const char* cameraName, const char* vehicleName);
bool(*SetCameraPose)(const char* cameraName, AirSimPose pose, const char* vehicleName);
bool(*SetCameraFoV)(const char* cameraName, const float fov_degrees, const char* vehicleName);
bool(*SetCameraDistortionParam)(const char* cameraName, const char* paramName, const float value, const char* vehicleName);
bool(*GetCameraDistortionParams)(const char* cameraName, const char* vehicleName);
bool(*SetSegmentationObjectId)(const char* meshName, int objectId, bool isNameRegex);
int(*GetSegmentationObjectId)(const char* meshName);
bool(*PrintLogMessage) (const char* message, const char* messageParam, const char* vehicleName, int severity);
UnityTransform(*GetTransformFromUnity)(const char* vehicleName);
bool(*Reset)(const char* vehicleName);
AirSimVector(*GetVelocity)(const char* vehicleName);
RayCastHitResult(*GetRayCastHit)(AirSimVector startVec, AirSimVector endVec, const char* vehicleName);
bool(*Pause)(const char* vehicleName, float timeScale);
UnityStringArray(*GetVehicleCameras)(const char* vehicleName);
bool(*UnityEnableVehicleCamera)(bool status);
bool(*UnityEnableVehicleRay)(bool status);

bool(*PrintTest) (const char* message);
bool(*AddVehicle)(const char* vehicleName, const char* vehicleType);
bool(*AddPedestrian)(const char* pedestrianName);
bool(*RemoveVehicle)(const char* vehicleName, const char* vehicleType);
bool(*RemovePedestrian)(const char* pedestrianName);
UnityStringArray(*GetVehicleTypesCall)();
UnityStringArray(*GetAllVehiclesListCall)();
UnityStringArray(*GetAllPedestriansListCall)();

bool(*SetPedestrianPose)(AirSimPose pose, bool ignoreCollision, const char* pedestrianName);
AirSimPose(*GetPedestrianPose)(const char* pedestrianName);
bool(*PedestrianReset)(const char* pedestrianName);
bool(*PedestrianSetEnableApi)(bool enableApi, const char* pedestrianName);
bool(*SetPedestrianApiControls)(AirSimUnity::PedestrianControls, const char* pedestrianName);
UnityStringArray(*GetPedestrianCameras)(const char* pedestrianName);



void InitVehicleManager(
	bool(*setPose)(AirSimPose pose, bool ignoreCollision, const char* vehicleName),
	AirSimPose(*getPose)(const char* vehicleName),
	AirSimCollisionInfo(*getCollisionInfo)(const char* vehicleName),
	AirSimRCData(*getDroneRCData)(const char* vehicleName),
	AirSimImageResponse(*getSimImages)(AirSimImageRequest request, const char* vehicleName),
	bool(*setRotorSpeed)(int rotorIndex, RotorInfo rotorInfo, const char* vehicleName),
	bool(*setEnableApi)(bool enableApi, const char* vehicleName),
	bool(*setCarApiControls)(msr::airlib::CarControls controls, const char* vehicleName),
	AirSimCarState(*getCarState)(const char* vehicleName),
	AirSimCameraInfo(*getCameraInfo)(const char* cameraName, const char* vehicleName),
	bool(*setCameraPose)(const char* cameraName, AirSimPose pose, const char* vehicleName),
	bool(*setCameraFoV)(const char* cameraName, const float fov_degrees, const char* vehicleName),
	bool(*setDistortionParam)(const char* cameraName, const char* paramName, const float value, const char* vehicleName),
	bool(*getDistortionParams)(const char* cameraName, const char* vehicleName),
	bool(*setSegmentationObjectId)(const char* meshName, int objectId, bool isNameRegex),
	int(*getSegmentationObjectId)(const char* meshName),
	bool(*printLogMessage) (const char* message, const char* messageParam, const char* vehicleName, int severity),
	UnityTransform(*getTransformFromUnity)(const char* vehicleName),
	bool(*reset)(const char* vehicleName),
	AirSimVector(*getVelocity)(const char* vehicleName),
	RayCastHitResult(*getRayCastHit)(AirSimVector startVec, AirSimVector endVec, const char* vehicleName),
	bool(*pause)(const char* vehicleName, float timeScale),
	UnityStringArray(*getVehicleCameras)(const char* vehicleName),
	bool(*unityEnableVehicleCamera)(bool status),
	bool(*unityEnableVehicleRay)(bool status)
)
{
	SetPose = setPose;
	GetPose = getPose;
	GetCollisionInfo = getCollisionInfo;
	GetRCData = getDroneRCData;
	GetSimImages = getSimImages;
	SetRotorSpeed = setRotorSpeed;
	SetEnableApi = setEnableApi;
	SetCarApiControls = setCarApiControls;
	GetCarState = getCarState;
	GetCameraInfo = getCameraInfo;
	SetCameraPose = setCameraPose;
	SetCameraFoV = setCameraFoV;
	SetCameraDistortionParam = setDistortionParam;
	GetCameraDistortionParams = getDistortionParams;
	SetSegmentationObjectId = setSegmentationObjectId;
	GetSegmentationObjectId = getSegmentationObjectId;
	PrintLogMessage = printLogMessage;
	GetTransformFromUnity = getTransformFromUnity;
	Reset = reset;
	GetVelocity = getVelocity;
	GetRayCastHit = getRayCastHit;
	Pause = pause;
	GetVehicleCameras = getVehicleCameras;
	UnityEnableVehicleCamera = unityEnableVehicleCamera;
	UnityEnableVehicleRay = unityEnableVehicleRay;
}


void InitServerManager(
	bool(*printTest) (const char* message),
	bool(*addVehicle)(const char* vehicleName, const char* vehicleType),
	bool(*addPedestrian)(const char* pedestrianName),
	bool(*removeVehicle)(const char* vehicleName, const char* vehicleType),
	bool(*removePedestrian)(const char* pedestrianName),
	UnityStringArray(*getVehicleTypes)(),
	UnityStringArray(*getAllVehiclesList)(),
	UnityStringArray(*getAllPedestriansList)()
) {
	PrintTest = printTest;
	AddVehicle = addVehicle;
	AddPedestrian = addPedestrian;
	RemoveVehicle = removeVehicle;
	RemovePedestrian = removePedestrian;
	GetVehicleTypesCall = getVehicleTypes;
	GetAllVehiclesListCall = getAllVehiclesList;
	GetAllPedestriansListCall = getAllPedestriansList;
}


void InitPedestrianManager(
	bool(*setPose)(AirSimPose pose, bool ignoreCollision, const char* pedestrianName),
	AirSimPose(*getPose)(const char* pedestrianName),
	bool(*reset)(const char* pedestrianName),
	bool(*setEnableApi)(bool enableApi, const char* pedestrianName),
	bool(*setPedestrianApiControls)(AirSimUnity::PedestrianControls, const char* pedestrianName),
	UnityStringArray(*getPedestrianCameras)(const char* pedestrianName)
) {
	SetPedestrianPose = setPose;
	GetPedestrianPose = getPose;
	PedestrianReset = reset;
	PedestrianSetEnableApi = setEnableApi;
	SetPedestrianApiControls = setPedestrianApiControls;
	GetPedestrianCameras = getPedestrianCameras;
}