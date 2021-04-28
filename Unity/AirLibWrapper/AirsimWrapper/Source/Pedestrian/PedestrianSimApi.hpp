#pragma once
#include "api/PedestrianSimApiBase.hpp"

class PedestrianSimApi : public msr::airlib::PedestrianSimApiBase
{
	virtual ~PedestrianSimApi() = default;
	virtual void printTest(const std::string& message) override;
	virtual msr::airlib::Pose getPose(const std::string& pedestrian_name) const override;
	virtual void setPose(const  msr::airlib::Pose& pose, bool ignore_collision, const std::string& pedestrian_name) override;
	virtual bool reset(const std::string& pedestrian_name) override;
	virtual bool enableApi(bool status, const std::string& pedestrian_name) override;
};
