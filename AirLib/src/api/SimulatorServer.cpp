
#include "api/SimulatorServer.hpp"
#include "rpc/server.h"
#include "common/Common.hpp"
#include "api/RpcLibAdaptorsBase.hpp"
#include "ServerUtils.h"

namespace msr {namespace airlib {


struct SimulatorServer::impl{
    impl(string server_address, uint16_t port)
        : server(server_address, port)
    {}

    impl(uint16_t port)
        : server(port)
    {}

    ~impl() {
    }

    void stop() {
        server.close_sessions();
        if (!is_async_) {
            // this deadlocks UI thread if async_run was called while there are pending rpc calls.
            server.stop();
        }
    }

    void run(bool block, std::size_t thread_count)
    {
        if (block) {
            server.run();
        }
        else {
            is_async_ = true;
            server.async_run(thread_count);
        }
    }

    rpc::server server;
    bool is_async_ = false;
};

//required for pimpl
SimulatorServer::~SimulatorServer()
{
    stop();
}

void SimulatorServer::start(bool block, std::size_t thread_count)
{
    pimpl_->run(block, thread_count);
}

void SimulatorServer::stop()
{
    pimpl_->stop();
}

void* SimulatorServer::getServer() const
{
    return &pimpl_->server;
}

SimulatorServer::SimulatorServer(ServerSimApiBase* serverptr, const string& ip_address = "localhost", uint16_t port = ServerPort) : ptr(serverptr){
    if (ip_address == "")
        pimpl_.reset(new impl(port));
    else
        pimpl_.reset(new impl(ip_address, port));

    pimpl_->server.bind("ping", [&]() -> bool { return true; });

    pimpl_->server.bind("simPrintTest", [&](const std::string& message) -> void {
        ptr->printTest(message);
    });

    // add Vehicle
    pimpl_->server.bind("simAddVehicle", [&](const std::string& vehicle_name, const std::string& vehicle_type,
        const msr::airlib_rpclib::RpcLibAdaptorsBase::Pose& pose, const std::string& pawn_path) -> bool {
            return ptr->addVehicle(vehicle_name, vehicle_type, pose.to(), pawn_path);
    });

    // Spawn Pedestrian
    pimpl_->server.bind("simAddPedestrian", [&](const std::string& pedestrian_name, const msr::airlib_rpclib::RpcLibAdaptorsBase::Pose& pose, const std::string& pawn_path) -> bool {
            return ptr->addPedestrian(pedestrian_name, pose.to(), pawn_path);
    });

    // Remove Vehicle
    pimpl_->server.bind("simRemoveVehicle", [&](const std::string& vehicle_name, const std::string& vehicle_type) -> bool {
        return ptr->removeVehicle(vehicle_name, vehicle_type);
    });

    // Remove Pedestrian
    pimpl_->server.bind("simRemovePedestrian", [&](const std::string& vehicle_name) -> bool {
        return ptr->removePedestrian(vehicle_name);
    });

    pimpl_->server.bind("simGetVehicleTypes", [&]()-> std::vector<std::string> {
        return ptr->GetVehicleTypes().data;
    });
}



}} // Namespace