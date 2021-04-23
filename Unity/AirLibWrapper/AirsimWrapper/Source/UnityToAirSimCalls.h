//#pragma once

#include <thread>
#include "UnityUtilities.hpp"
#include "SimHUD/SimHUD.h"
#include "SimHUD/ServerHUD.hpp"
#include "Pedestrian/PedestrianHUD.hpp"
#include "Logger.h"


#ifdef _WIN32
	#define EXPORT __declspec(dllexport)
#else
	#define EXPORT __attribute__((visibility("default")))
#endif

static SimHUD* key = nullptr;
static ServerHUD* serverKey = nullptr;
static PedestrianHUD* pedestrianKey = nullptr;

void StartMainServerThread(int port_number);
void StartServerThread(std::string vehicle_name, std::string sim_mode_name, int port_number);
void StartPedestrianServerThread(int port);

extern "C" EXPORT bool StartMainServer(int port_number)
{
	LOGGER->WriteLog("Starting main server");
	std::thread server_thread(StartMainServerThread, port_number);
	server_thread.detach();
	int waitCounter = 25; // waiting for maximum 5 seconds to start a server.
	while ((serverKey == nullptr || !serverKey->server_started_Successfully_) && waitCounter > 0)
	{
		std::this_thread::sleep_for(std::chrono::milliseconds(200));
		waitCounter--;
	}
	return serverKey->server_started_Successfully_;
}

extern "C" EXPORT void StopMainServer()
{
	serverKey->EndPlay();
	if (serverKey != nullptr)
	{
		delete serverKey;
		serverKey = nullptr;
	}
	LOGGER->WriteLog("Server stopped");
}


extern "C" EXPORT bool StartServer(char* vehicle_name, char* sim_mode_name, int port_number)
{
	LOGGER->WriteLog("Starting vehicle server for : " + std::string(sim_mode_name));
	std::thread server_thread(StartServerThread, vehicle_name, sim_mode_name, port_number);
	server_thread.detach();
	int waitCounter = 25; // waiting for maximum 5 seconds to start a server.
	while ((key == nullptr || !key->server_started_Successfully_) && waitCounter > 0)
	{
		std::this_thread::sleep_for(std::chrono::milliseconds(200));
		waitCounter--;
	}
	return key->server_started_Successfully_;
}

extern "C" EXPORT void StopServer(char* vehicle_name)
{
	key->EndPlay();
	if (key != nullptr)
	{
		delete key;
		key = nullptr;
	}
		LOGGER->WriteLog("Server stopped");
}

extern "C" EXPORT void CallTick(float deltaSeconds)
{
	key->Tick(deltaSeconds);
}

extern "C" EXPORT void InvokeCollisionDetection(AirSimUnity::AirSimCollisionInfo collision_info)
{
	auto simMode = key->GetSimMode();
	//LOGGER->WriteLog("This is a test");
	if (simMode)
	{
		auto vehicleApi = simMode->getVehicleSimApi(simMode->vehicle_name_);
		if (vehicleApi)
		{
			msr::airlib::CollisionInfo collisionInfo = UnityUtilities::Convert_to_AirSimCollisioinInfo(collision_info);
			vehicleApi->OnCollision(collisionInfo);
		}
	}
}

extern "C" EXPORT void StoreImage(char* vehicle_name, char* camera, AirSimUnity::AirSimImageResponse img)
{
	auto simMode = key->GetSimMode();

	if (simMode) {
		auto worldPtr = simMode->getWorldSimApiBase();
		if (worldPtr){
			msr::airlib::ImageCaptureBase::ImageResponse reponse;
			UnityUtilities::Convert_to_AirsimResponse(img, reponse, camera);
			worldPtr->storeImage(vehicle_name, camera, reponse);
		}
	}

}


extern "C" EXPORT bool StartPedestrianServer(int port_number)
{
	LOGGER->WriteLog("Starting pedestrianserver server on port: " + port_number);
	std::thread server_thread(StartPedestrianServerThread, port_number);
	server_thread.detach();
	int waitCounter = 25; // waiting for maximum 5 seconds to start a server.
	while ((pedestrianKey == nullptr || !pedestrianKey->server_started_Successfully_) && waitCounter > 0)
	{
		std::this_thread::sleep_for(std::chrono::milliseconds(200));
		waitCounter--;
	}
	return pedestrianKey->server_started_Successfully_;
}

extern "C" EXPORT void StopPedestrianServer()
{
	pedestrianKey->EndPlay();
	if (pedestrianKey != nullptr)
	{
		delete pedestrianKey;
		pedestrianKey = nullptr;
	}
	LOGGER->WriteLog("Server stopped");
}
