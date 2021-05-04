#pragma once

#include "common/CommonStructs.hpp"
#include "common/Common.hpp"
#include "api/RpcLibServerBase.hpp"
#include "ServerUtils.h"

namespace msr {	namespace airlib {

	class ServerSimApiBase {
	public:
		virtual ~ServerSimApiBase() = default;
		virtual void printTest(const std::string& message) = 0;
		virtual bool addVehicle(const std::string& vehicle_name, const std::string& vehicle_type, const msr::airlib::Pose& pose, const std::string& pawn_path) = 0;
		virtual bool addPedestrian(const std::string& pedestrian_name, const msr::airlib::Pose& pose, const std::string& pawn_path) = 0;
		virtual bool removeVehicle(const std::string& vehicle_name, const std::string& vehicle_type) = 0;
		virtual bool removePedestrian(const std::string& pedestrian_name) = 0;
		virtual msr::airlib::VehicleTypes GetVehicleTypes() = 0;
	};
	
}} // Namespace