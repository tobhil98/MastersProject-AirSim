
#include "api/PedestrianServer.hpp"
#include "rpc/server.h"
#include "common/Common.hpp"
#include "api/RpcLibAdaptorsBase.hpp"


namespace msr {
    namespace airlib {


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

            pimpl_->server.bind("ping", [&]() -> bool { return true; });


        }



    }
} // Namespace