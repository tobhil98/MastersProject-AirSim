#pragma once

#include "common/CommonStructs.hpp"
#include "common/Common.hpp"
#include "api/RpcLibServerBase.hpp"

namespace msr {
	namespace airlib {



		class PedestrianSimApiBase {
		public:
			virtual ~PedestrianSimApiBase() = default;
			virtual void printTest(const std::string& message) = 0;
		};

	}
} // Namespace