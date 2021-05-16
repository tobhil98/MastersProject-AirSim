#pragma once
#include "rpc/server.h"
#include "common/Common.hpp"
#include "api/ApiServerBase.hpp"
#include "api/PedestrianSimApiBase.hpp"


namespace msr {
    namespace airlib {

        class PedestrianServer : public ApiServerBase
        {
        public:
            //void startServer()
            PedestrianServer(PedestrianSimApiBase* serverptr, const string& ip_address, uint16_t port);
            virtual ~PedestrianServer();    //required for pimpl

            virtual void start(bool block, std::size_t thread_count) override;
            virtual void stop() override;

        protected:
            void* getServer() const;

        private:
            struct impl;
            std::unique_ptr<impl> pimpl_;
            std::unique_ptr<PedestrianSimApiBase> ptr;


        };

    }
}