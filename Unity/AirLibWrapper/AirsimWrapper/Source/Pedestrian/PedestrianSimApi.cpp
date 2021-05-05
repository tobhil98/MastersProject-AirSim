#include "PedestrianSimApi.hpp"
#include "../Logger.h"
#include "../PInvokeWrapper.h"
#include "../UnityUtilities.hpp"
#include "../AirSimStructs.hpp"

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

bool PedestrianSimApi::setPedestrianControls(const msr::airlib::PedestrianControls& controls, const std::string& pedestrian_name)
{
	AirSimUnity::PedestrianControls c;
	c.speed = controls.speed;
	c.steering = controls.steering;
	return SetPedestrianApiControls(c, pedestrian_name.c_str());
}

std::vector<msr::airlib::ImageCaptureBase::ImageResponse> PedestrianSimApi::getImages(
    const std::vector<msr::airlib::ImageCaptureBase::ImageRequest>& requests, const std::string& pedestrian_name)
{
    std::string camera = requests[0].camera_name;
    std::vector<msr::airlib::ImageCaptureBase::ImageResponse> responses;
    try {
        responses.push_back(PedestrianMap[pedestrian_name].ResponseMap[camera]);
    }
    catch (...)
    {
        LOGGER->WriteLog("Invalid lookup for " + pedestrian_name);
    }
    return responses;
}

void PedestrianSimApi::storeImage(const std::string& pedestrian_name, const std::string& camera_name, msr::airlib::ImageCaptureBase::ImageResponse img)
{
	if (pedestrian_name != "" && camera_name != "")
		PedestrianMap[pedestrian_name].ResponseMap[camera_name] = img;
}


msr::airlib::StringArray PedestrianSimApi::getCameras(const std::string& pedestrian_name)
{
    AirSimUnity::UnityStringArray rawTypes = GetPedestrianCameras(pedestrian_name.c_str());
    msr::airlib::StringArray out;
    UnityUtilities::Convert_to_AirSimStringArray(rawTypes, out);
    return out;
}