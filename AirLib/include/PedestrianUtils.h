#pragma once
#include "rpc/msgpack.hpp"

namespace msr {
	namespace airlib {

        struct PedestrianControls {
            float speed = 0; /* 1 to -1 */
            float steering = 0; /* 1 to -1 */


            PedestrianControls()
            {
            }
            PedestrianControls(float speed_val, float steering_val)
                : speed(speed_val), steering(steering_val)
            {
            }

            PedestrianControls(const msr::airlib::PedestrianControls& s)
            {
                speed = s.speed;
                steering = s.steering;
            }

            msr::airlib::PedestrianControls to() const
            {
                return msr::airlib::PedestrianControls(speed, steering);
            }

            MSGPACK_DEFINE_MAP(speed, steering);

        };


	}
}