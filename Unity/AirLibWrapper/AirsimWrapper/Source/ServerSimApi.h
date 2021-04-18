#pragma once

#include "api/ServerSimApiBase.hpp"


class ServerSimApi : public msr::airlib::ServerSimApiBase
{
	virtual ~ServerSimApi() = default;
	virtual void printTest(const std::string& message) override;
	virtual bool addVehicle(const std::string& vehicle_name, const std::string& vehicle_type, const msr::airlib::Pose& pose, const std::string& pawn_path) override;
};
