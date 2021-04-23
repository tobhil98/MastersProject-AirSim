#pragma once
#include "api/PedestrianSimApiBase.hpp"

class PedestrianSimApi : public msr::airlib::PedestrianSimApiBase
{
	virtual ~PedestrianSimApi() = default;
	virtual void printTest(const std::string& message) override;

};
