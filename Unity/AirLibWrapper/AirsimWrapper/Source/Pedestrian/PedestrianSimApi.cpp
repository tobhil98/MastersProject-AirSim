#include "PedestrianSimApi.hpp"
#include "../Logger.h"
#include "../PInvokeWrapper.h"

void PedestrianSimApi::printTest(const std::string& message)
{
    PrintTest(message.c_str());
}

