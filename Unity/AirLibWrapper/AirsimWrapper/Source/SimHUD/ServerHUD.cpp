#include "ServerHUD.hpp"
#include "../PInvokeWrapper.h"
#include "../ServerSimApi.h"


ServerHUD::ServerHUD(int port_number) : port_number_(port_number)
{
	server_started_Successfully_ = false;
}

void ServerHUD::BeginPlay()
{
	startApiServer();
	server_sim_api_.reset(new ServerSimApi());
}

void ServerHUD::Tick(float DeltaSeconds)
{ }

void ServerHUD::EndPlay()
{
	stopApiServer();
}

void ServerHUD::startApiServer()
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
void ServerHUD::stopApiServer()
{
	if (api_server_ != nullptr) {
		api_server_->stop();
		api_server_.reset(nullptr);
	}
}

std::unique_ptr<msr::airlib::ApiServerBase> ServerHUD::createApiServer() const
{
#ifdef AIRLIB_NO_RPC
	return ASimModeBase::createApiServer();
#else
	return std::unique_ptr<msr::airlib::ApiServerBase>(new msr::airlib::SimulatorServer(new ServerSimApi(), msr::airlib::AirSimSettings::singleton().api_server_address, port_number_));
#endif
}