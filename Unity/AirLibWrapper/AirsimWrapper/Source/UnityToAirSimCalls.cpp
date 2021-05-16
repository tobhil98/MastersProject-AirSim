#include "UnityToAirSimCalls.h"

void StartServerThread(std::string sim_mode_name, int port_number)
{
	key = new SimHUD(sim_mode_name, port_number);
	key->BeginPlay();
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