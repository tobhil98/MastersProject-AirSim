
#include "api/PedestrianServer.hpp"
#include "rpc/server.h"
#include "common/Common.hpp"
#include "api/RpcLibAdaptorsBase.hpp"
#include "PedestrianUtils.h"

namespace msr {
    namespace airlib {

        typedef msr::airlib_rpclib::RpcLibAdaptorsBase RpcLibAdaptorsBase;


        struct PedestrianServer::impl {
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
        PedestrianServer::~PedestrianServer()
        {
            stop();
        }

        void PedestrianServer::start(bool block, std::size_t thread_count)
        {
            pimpl_->run(block, thread_count);
        }

        void PedestrianServer::stop()
        {
            pimpl_->stop();
        }

        void* PedestrianServer::getServer() const
        {
            return &pimpl_->server;
        }

        PedestrianServer::PedestrianServer(PedestrianSimApiBase* serverptr, const string& ip_address = "localhost", uint16_t port = PedestrianPort) : ptr(serverptr) {
            if (ip_address == "")
                pimpl_.reset(new impl(port));
            else
                pimpl_.reset(new impl(ip_address, port));

            pimpl_->server.bind("PedestrianPing", [&]() -> bool { return true; });
            pimpl_->server.bind("getServerVersion", [&]() -> int { return 5; });

            pimpl_->server.bind("SetPedestrianPose", [&](const RpcLibAdaptorsBase::Pose& pose, bool ignore_collision, const std::string& pedestrian_name) -> void {
               ptr->setPose(pose.to(), ignore_collision, pedestrian_name);
            });

            pimpl_->server.bind("GetPedestrianPose", [&](const std::string& pedestrian_name) -> RpcLibAdaptorsBase::Pose {
                const auto& pose = ptr->getPose(pedestrian_name);
                return RpcLibAdaptorsBase::Pose(pose);
            });

            pimpl_->server.bind("PedestrianReset", [&](const std::string& pedestrian_name) -> bool {
                return ptr->reset(pedestrian_name);
            });

            pimpl_->server.bind("PedestrianEnableApiControl", [&](bool is_enabled, const std::string& pedestrian_name) -> bool {
                return ptr->enableApi(is_enabled, pedestrian_name);
            });

            pimpl_->server.bind("setPedestrianControls", [&](const msr::airlib::PedestrianControls& controls, const std::string& pedestrian_name) -> bool {
                return ptr->setPedestrianControls(controls, pedestrian_name);
            });

            pimpl_->server.bind("simGetImages", [&](const std::vector<RpcLibAdaptorsBase::ImageRequest>& request_adapter, const std::string& vehicle_name) ->
                vector<RpcLibAdaptorsBase::ImageResponse> {
                    //const auto& response = getVehicleSimApi(vehicle_name)->getImages(RpcLibAdaptorsBase::ImageRequest::to(request_adapter));
                    const auto& response = ptr->getImages(RpcLibAdaptorsBase::ImageRequest::to(request_adapter), vehicle_name);
                    return RpcLibAdaptorsBase::ImageResponse::from(response);
                });


        }



    }
} // Namespace