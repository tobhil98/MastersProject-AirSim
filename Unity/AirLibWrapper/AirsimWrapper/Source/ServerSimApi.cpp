#include "PInvokeWrapper.h"
#include "ServerSimApi.h"

void ServerSimApi::printTest(const std::string& message)
{
    PrintTest(message.c_str());
}


bool ServerSimApi::addVehicle(const std::string& vehicle_name, const std::string& vehicle_type, const msr::airlib::Pose& pose, const std::string& pawn_path)
{
    unused(pose);
    unused(pawn_path);

    // Add element to map
    AddVehicle(vehicle_name.c_str(), vehicle_type.c_str());
    return true;
}