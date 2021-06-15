#include "PInvokeWrapper.h"
#include "ServerSimApi.h"
#include "Logger.h"
#include "UnityUtilities.hpp"

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

bool ServerSimApi::addPedestrian(const std::string& pedestrianName, const msr::airlib::Pose& pose, const std::string& pawn_path)
{
    unused(pose);
    unused(pawn_path);

    // Add element to map
    AddPedestrian(pedestrianName.c_str());
    return true;
}

bool ServerSimApi::removeVehicle(const std::string& vehicle_name, const std::string& vehicle_type)
{
    RemoveVehicle(vehicle_name.c_str(), vehicle_type.c_str());
    return true;
}

bool ServerSimApi::removePedestrian(const std::string& vehicle_name)
{
    RemovePedestrian(vehicle_name.c_str());
    return true;
}

msr::airlib::StringArray ServerSimApi::getVehicleTypes()
{
    AirSimUnity::UnityStringArray rawTypes = GetVehicleTypesCall();
    msr::airlib::StringArray out;
    UnityUtilities::Convert_to_AirSimStringArray(rawTypes, out);
    return out;
}


msr::airlib::StringArray ServerSimApi::getAllVehiclesList()
{
    AirSimUnity::UnityStringArray rawTypes = GetAllVehiclesListCall();
    msr::airlib::StringArray out;
    UnityUtilities::Convert_to_AirSimStringArray(rawTypes, out);
    return out;
}

msr::airlib::StringArray ServerSimApi::getAllPedestriansList()
{
    AirSimUnity::UnityStringArray rawTypes = GetAllPedestriansListCall();
    msr::airlib::StringArray out;
    UnityUtilities::Convert_to_AirSimStringArray(rawTypes, out);
    return out;
}