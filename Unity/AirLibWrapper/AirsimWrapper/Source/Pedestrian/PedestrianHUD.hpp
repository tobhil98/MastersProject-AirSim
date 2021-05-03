#pragma once

#include "common/AirSimSettings.hpp"
#include "api/ApiServerBase.hpp"
#include "api/PedestrianServer.hpp"
#include "api/PedestrianSimApiBase.hpp"

class PedestrianHUD {
public:
	PedestrianHUD(int port_number);
	void BeginPlay();
	void Tick(float DeltaSeconds);
	void EndPlay();
	msr::airlib::PedestrianSimApiBase* GetPedestrianSimApiBasePtr();
private:
	void startApiServer();
	void stopApiServer();
	std::unique_ptr<msr::airlib::ApiServerBase> createApiServer() const;


public:
	bool server_started_Successfully_;
private:
	int port_number_;
	std::unique_ptr<msr::airlib::ApiServerBase> api_server_;
	std::unique_ptr<msr::airlib::PedestrianSimApiBase> server_sim_api_;

};