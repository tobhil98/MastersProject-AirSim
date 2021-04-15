#pragma once
#include "common/AirSimSettings.hpp"
#include "api/ApiServerBase.hpp"
#include "api/SimulatorServer.hpp"

class ServerHUD {
public:
	ServerHUD(int port_number);
	void BeginPlay();
	void Tick(float DeltaSeconds);
	void EndPlay();
private:
	void startApiServer();
	void stopApiServer();
	std::unique_ptr<msr::airlib::ApiServerBase> createApiServer() const;


public:
	bool server_started_Successfully_;
private:
	int port_number_;
	std::unique_ptr<msr::airlib::ApiServerBase> api_server_;

};