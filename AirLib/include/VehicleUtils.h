#pragma once

#include "rpc/msgpack.hpp"

// I:\Simulators\AirSim\Unity\AirLibWrapper\AirsimWrapper\Source\AirSimStructs.hpp


namespace msr { namespace airlib {

    struct CarControls {
        float throttle = 0; /* 1 to -1 */
        float steering = 0; /* 1 to -1 */
        float brake = 0;    /* 1 to -1 */
        bool handbrake = false;
        bool is_manual_gear = false;
        int manual_gear = 0;
        bool gear_immediate = true;

        CarControls()
        {
        }
        CarControls(float throttle_val, float steering_val, float brake_val, bool handbrake_val,
            bool is_manual_gear_val, int manual_gear_val, bool gear_immediate_val)
            : throttle(throttle_val), steering(steering_val), brake(brake_val), handbrake(handbrake_val),
            is_manual_gear(is_manual_gear_val), manual_gear(manual_gear_val), gear_immediate(gear_immediate_val)
        {
        }

        CarControls(const msr::airlib::CarControls& s)
        {
            throttle = s.throttle;
            steering = s.steering;
            brake = s.brake;
            handbrake = s.handbrake;
            is_manual_gear = s.is_manual_gear;
            manual_gear = s.manual_gear;
            gear_immediate = s.gear_immediate;
        }

        msr::airlib::CarControls to() const
        {
            return msr::airlib::CarControls(throttle, steering, brake, handbrake,
                is_manual_gear, manual_gear, gear_immediate);
        }

        MSGPACK_DEFINE_MAP(throttle, steering, brake, handbrake, is_manual_gear, manual_gear, gear_immediate);

        void set_throttle(float throttle_val, bool forward)
        {
            if (forward) {
                is_manual_gear = false;
                manual_gear = 0;
                throttle = std::abs(throttle_val);
            }
            else {
                is_manual_gear = false;
                manual_gear = -1;
                throttle = -std::abs(throttle_val);
            }
        }


    };


    struct CarEntity {
        std::string name;
        CarControls controls;
        //State?
    };

}} // Namespace