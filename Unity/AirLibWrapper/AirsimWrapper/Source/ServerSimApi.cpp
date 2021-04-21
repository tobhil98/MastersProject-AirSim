#include "PInvokeWrapper.h"
#include "ServerSimApi.h"
#include "Logger.h"

void ServerSimApi::printTest(const std::string& message)
{
    PrintTest(message.c_str());
}


bool ServerSimApi::addVehicle(const std::string& vehicle_name, const std::string& vehicle_type, const msr::airlib::Pose& pose, const std::string& pawn_path)
{
    unused(pose);
    unused(pawn_path);

    // Add element to map
    LOGGER->WriteLog("Add vehicle - " + vehicle_name);
    AddVehicle(vehicle_name.c_str(), vehicle_type.c_str());
    LOGGER->WriteLog("Vehicle added - " + vehicle_name);
    return true;
}

bool ServerSimApi::removeVehicle(const std::string& vehicle_name, const std::string& vehicle_type)
{
    RemoveVehicle(vehicle_name.c_str(), vehicle_type.c_str());
    return true;
}
