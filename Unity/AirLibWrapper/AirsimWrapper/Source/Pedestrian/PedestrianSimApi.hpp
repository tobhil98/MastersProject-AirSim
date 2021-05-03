#pragma once
#include "api/PedestrianSimApiBase.hpp"

class PedestrianSimApi : public msr::airlib::PedestrianSimApiBase
{
private:

	virtual ~PedestrianSimApi() = default;
	virtual void printTest(const std::string& message) override;
	virtual msr::airlib::Pose getPose(const std::string& pedestrian_name) const override;
	virtual void setPose(const  msr::airlib::Pose& pose, bool ignore_collision, const std::string& pedestrian_name) override;
	virtual bool reset(const std::string& pedestrian_name) override;
	virtual bool enableApi(bool status, const std::string& pedestrian_name) override;
	virtual bool setPedestrianControls(const msr::airlib::PedestrianControls& controls, const std::string& pedestrian_name) override;
	virtual std::vector<msr::airlib::ImageCaptureBase::ImageResponse> getImages(
		const std::vector<msr::airlib::ImageCaptureBase::ImageRequest>& requests, const std::string& pedestrian_name) override;

public:
	virtual void storeImage(const std::string& vehicle_name, const std::string& camera_name, msr::airlib::ImageCaptureBase::ImageResponse img) override;

private:
	std::unordered_map<std::string, msr::airlib::PedestrianEntity> PedestrianMap;

};
