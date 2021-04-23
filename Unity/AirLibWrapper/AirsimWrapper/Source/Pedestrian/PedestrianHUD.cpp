#pragma once

#include "PedestrianHUD.hpp"

#include "../PInvokeWrapper.h"
#include "../Pedestrian/PedestrianSimApi.hpp"


PedestrianHUD::PedestrianHUD(int port_number) : port_number_(port_number)
{
	server_started_Successfully_ = false;
}

void PedestrianHUD::BeginPlay()
{
	startApiServer();
	server_sim_api_.reset(new PedestrianSimApi());
}

void PedestrianHUD::Tick(float DeltaSeconds)
{ }

void PedestrianHUD::EndPlay()
{
	stopApiServer();
}

void PedestrianHUD::startApiServer()
{
	if (msr::airlib::AirSimSettings::singleton().enable_rpc) {
		api_server_ = createApiServer();
		try {
			api_server_->start(false, 4);		// TODOME set to 1?
			server_started_Successfully_ = true;
		}
		catch (std::exception& ex) {
			std::string str = "Cannot start RpcLib Server: " + std::string(ex.what());
			PrintTest(str.c_str());
		}
	}
	else
		PrintTest("API server is disabled in settings");
}
void PedestrianHUD::stopApiServer()
{
	if (api_server_ != nullptr) {
		api_server_->stop();
		api_server_.reset(nullptr);
	}
}

std::unique_ptr<msr::airlib::ApiServerBase> PedestrianHUD::createApiServer() const
{
#ifdef AIRLIB_NO_RPC
	return ASimModeBase::createApiServer();
#else
	return std::unique_ptr<msr::airlib::ApiServerBase>(new msr::airlib::PedestrianServer(new PedestrianSimApi(), msr::airlib::AirSimSettings::singleton().api_server_address, port_number_));
#endif
}