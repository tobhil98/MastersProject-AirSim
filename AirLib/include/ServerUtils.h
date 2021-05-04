#pragma once
#pragma once
#include "rpc/msgpack.hpp"


namespace msr {
    namespace airlib {

        struct VehicleTypes {
            std::vector<std::string> data;
            int elements;


            VehicleTypes()
            {
            }

            MSGPACK_DEFINE_MAP(data, elements);
            // Reset?

        };


    }
}