#pragma once

#include "common/CommonStructs.hpp"
#include "common/Common.hpp"
#include "api/RpcLibServerBase.hpp"
#include "PedestrianUtils.h"
#include "ServerUtils.h"

namespace msr {
	namespace airlib {



		class PedestrianSimApiBase {
		public:
			virtual ~PedestrianSimApiBase() = default;
			virtual void printTest(const std::string& message) = 0;
			virtual msr::airlib::Pose getPose(const std::string& pedestrian_name) const = 0;
			virtual void setPose(const  msr::airlib::Pose& pose, bool ignore_collision, const std::string& pedestrian_name) = 0;
			virtual bool reset(const std::string& pedestrian_name) = 0;
			virtual bool enableApi(bool status, const std::string& pedestrian_name) = 0;
			virtual bool setPedestrianControls(const msr::airlib::PedestrianControls& controls, const std::string& pedestrian_name) = 0;
			virtual void storeImage(const std::string& vehicle_name, const std::string& camera_name, msr::airlib::ImageCaptureBase::ImageResponse img) = 0;
			virtual std::vector<msr::airlib::ImageCaptureBase::ImageResponse> getImages(
				const std::vector<msr::airlib::ImageCaptureBase::ImageRequest>& requests, const std::string& pedestrian_name) = 0;
			virtual msr::airlib::StringArray getCameras(const std::string& pedestrian_name) = 0;
		};

	}
} // Namespace