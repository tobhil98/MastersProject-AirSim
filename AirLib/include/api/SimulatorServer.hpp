#pragma once
#include "rpc/server.h"
#include "common/Common.hpp"
#include "api/ApiServerBase.hpp"


namespace msr { namespace airlib {

class SimulatorServer : public ApiServerBase
{
public:
	//void startServer()
    SimulatorServer(const string& ip_address, uint16_t port);
    virtual ~SimulatorServer();    //required for pimpl

    virtual void start(bool block, std::size_t thread_count) override;
    virtual void stop() override;

protected:
    void* getServer() const;

private:
	struct impl;
	std::unique_ptr<impl> pimpl_;

};

}}