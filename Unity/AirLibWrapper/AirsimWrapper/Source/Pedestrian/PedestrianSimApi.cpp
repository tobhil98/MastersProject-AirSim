#include "PedestrianSimApi.hpp"
#include "../Logger.h"
#include "../PInvokeWrapper.h"
#include "../UnityUtilities.hpp"

void PedestrianSimApi::printTest(const std::string& message)
{
    PrintTest(message.c_str());
}

msr::airlib::Pose PedestrianSimApi::getPose(const std::string& pedestrian_name) const
{
	AirSimUnity::AirSimPose airSimPose = GetPedestrianPose(pedestrian_name.c_str());
	return UnityUtilities::Convert_to_Pose(airSimPose);
}

void PedestrianSimApi::setPose(const  msr::airlib::Pose& pose, bool ignore_collision, const std::string& pedestrian_name)
{
	SetPedestrianPose(UnityUtilities::Convert_to_AirSimPose(pose), ignore_collision, pedestrian_name.c_str());
}

bool PedestrianSimApi::reset(const std::string& pedestrian_name)
{
	return PedestrianReset(pedestrian_name.c_str());
}

bool PedestrianSimApi::enableApi(bool status, const std::string& pedestrian_name)
{
	return PedestrianSetEnableApi(status, pedestrian_name.c_str());
}