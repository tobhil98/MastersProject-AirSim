#include "UnityToAirSimCalls.h"

void StartServerThread(std::string vehicle_name, std::string sim_mode_name, int port_number)
{
	key = new SimHUD(vehicle_name, sim_mode_name, port_number);
	//LOGGER->WriteLog("SimHUD created");
	key->BeginPlay();
	//LOGGER->WriteLog("BeginPlay done");

}

void StartMainServerThread(int port_number)
{
	serverKey = new ServerHUD(port_number);
	serverKey->BeginPlay();
}

void StartPedestrianServerThread(int port_number)
{
	pedestrianKey = new PedestrianHUD(port_number);
	pedestrianKey->BeginPlay();
}