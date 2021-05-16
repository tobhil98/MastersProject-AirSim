#pragma once
#pragma once
#include "rpc/msgpack.hpp"


namespace msr {
    namespace airlib {

        struct StringArray {
            std::vector<std::string> data;
            int elements;


            StringArray()
            {
            }

            MSGPACK_DEFINE_MAP(data, elements);
            // Reset?

        };


    }
}