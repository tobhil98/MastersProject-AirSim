#pragma once

#include "api/ServerSimApiBase.hpp"


class ServerSimApi : public msr::airlib::ServerSimApiBase
{
	virtual ~ServerSimApi() = default;
	virtual void printTest(const std::string& message) override;
	virtual bool addVehicle(const std::string& vehicle_name, const std::string& vehicle_type, const msr::airlib::Pose& pose, const std::string& pawn_path) override;
	virtual bool addPedestrian(const std::string& pedestrian_name, const msr::airlib::Pose& pose, const std::string& pawn_path) override;
	virtual bool removeVehicle(const std::string& vehicle_name, const std::string& vehicle_type) override;
	virtual bool removePedestrian(const std::string& pedestrian_name) override;
	virtual msr::airlib::StringArray getVehicleTypes() override;
	virtual msr::airlib::StringArray getAllVehiclesList() override;
	virtual msr::airlib::StringArray getAllPedestriansList() override;
};
