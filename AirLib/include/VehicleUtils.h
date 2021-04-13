#pragma once

#include "rpc/msgpack.hpp"


// I:\Simulators\AirSim\Unity\AirLibWrapper\AirsimWrapper\Source\AirSimStructs.hpp

// I:\Simulators\AirSim\AirLib\include\api\RpcLibAdaptorsBase.hpp


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

    struct ImageRequest {
        std::string camera_name;
        ImageCaptureBase::ImageType image_type = ImageCaptureBase::ImageType::Scene;
        bool pixels_as_float = false;
        bool compress = true;

        ImageRequest()
        {}

        ImageRequest(const std::string& camera_name_val, ImageCaptureBase::ImageType image_type_val, bool pixels_as_float_val = false, bool compress_val = true)
        {
            camera_name = camera_name_val;
            image_type = image_type_val;
            pixels_as_float = pixels_as_float_val;
            compress = compress_val;
        }
    };

    struct ImageResponse {
        vector<uint8_t> image_data_uint8;
        vector<float> image_data_float;

        std::string camera_name;
        Vector3r camera_position = Vector3r::Zero();
        Quaternionr camera_orientation = Quaternionr::Identity();
        TTimePoint time_stamp = 0;
        std::string message;
        bool pixels_as_float = false;
        bool compress = true;
        int width = 0, height = 0;
        ImageCaptureBase::ImageType image_type;
    };


    //struct ImageRequest {
    //    std::string camera_name;
    //    msr::airlib::ImageCaptureBase::ImageType image_type;
    //    bool pixels_as_float;
    //    bool compress;

    //    MSGPACK_DEFINE_MAP(camera_name, image_type, pixels_as_float, compress);

    //    ImageRequest()
    //    {}

    //    ImageRequest(const msr::airlib::ImageCaptureBase::ImageRequest& s)
    //    {
    //        camera_name = s.camera_name;
    //        image_type = s.image_type;
    //        pixels_as_float = s.pixels_as_float;
    //        compress = s.compress;
    //    }

    //    msr::airlib::ImageCaptureBase::ImageRequest to() const
    //    {
    //        msr::airlib::ImageCaptureBase::ImageRequest d;
    //        d.camera_name = camera_name;
    //        d.image_type = image_type;
    //        d.pixels_as_float = pixels_as_float;
    //        d.compress = compress;

    //        return d;
    //    }

    //    static std::vector<ImageRequest> from(
    //        const std::vector<msr::airlib::ImageCaptureBase::ImageRequest>& request
    //    ) {
    //        std::vector<ImageRequest> request_adaptor;
    //        for (const auto& item : request)
    //            request_adaptor.push_back(ImageRequest(item));

    //        return request_adaptor;
    //    }
    //    static std::vector<msr::airlib::ImageCaptureBase::ImageRequest> to(
    //        const std::vector<ImageRequest>& request_adapter
    //    ) {
    //        std::vector<msr::airlib::ImageCaptureBase::ImageRequest> request;
    //        for (const auto& item : request_adapter)
    //            request.push_back(item.to());

    //        return request;
    //    }
    //};

    //struct ImageResponse {
    //    std::vector<uint8_t> image_data_uint8;
    //    std::vector<float> image_data_float;

    //    std::string camera_name;
    //    Vector3r camera_position;
    //    Quaternionr camera_orientation;
    //    msr::airlib::TTimePoint time_stamp;
    //    std::string message;
    //    bool pixels_as_float;
    //    bool compress;
    //    int width, height;
    //    msr::airlib::ImageCaptureBase::ImageType image_type;

    //    MSGPACK_DEFINE_MAP(image_data_uint8, image_data_float, camera_position, camera_name,
    //        camera_orientation, time_stamp, message, pixels_as_float, compress, width, height, image_type);

    //    ImageResponse()
    //    {}

    //    ImageResponse(const msr::airlib::ImageCaptureBase::ImageResponse& s)
    //    {
    //        pixels_as_float = s.pixels_as_float;

    //        image_data_uint8 = s.image_data_uint8;
    //        image_data_float = s.image_data_float;

    //        camera_name = s.camera_name;
    //        camera_position = Vector3r(s.camera_position);
    //        camera_orientation = Quaternionr(s.camera_orientation);
    //        time_stamp = s.time_stamp;
    //        message = s.message;
    //        compress = s.compress;
    //        width = s.width;
    //        height = s.height;
    //        image_type = s.image_type;
    //    }

    //    msr::airlib::ImageCaptureBase::ImageResponse to() const
    //    {
    //        msr::airlib::ImageCaptureBase::ImageResponse d;

    //        d.pixels_as_float = pixels_as_float;

    //        if (!pixels_as_float)
    //            d.image_data_uint8 = image_data_uint8;
    //        else
    //            d.image_data_float = image_data_float;

    //        d.camera_name = camera_name;
    //        d.camera_position = camera_position.to();
    //        d.camera_orientation = camera_orientation.to();
    //        d.time_stamp = time_stamp;
    //        d.message = message;
    //        d.compress = compress;
    //        d.width = width;
    //        d.height = height;
    //        d.image_type = image_type;

    //        return d;
    //    }

    //    static std::vector<msr::airlib::ImageCaptureBase::ImageResponse> to(
    //        const std::vector<ImageResponse>& response_adapter
    //    ) {
    //        std::vector<msr::airlib::ImageCaptureBase::ImageResponse> response;
    //        for (const auto& item : response_adapter)
    //            response.push_back(item.to());

    //        return response;
    //    }
    //    static std::vector<ImageResponse> from(
    //        const std::vector<msr::airlib::ImageCaptureBase::ImageResponse>& response
    //    ) {
    //        std::vector<ImageResponse> response_adapter;
    //        for (const auto& item : response)
    //            response_adapter.push_back(ImageResponse(item));

    //        return response_adapter;
    //    }
    //};



    struct CarEntity {
        std::string name;
        CarControls controls;
        //State?
        std::unordered_map<std::string, msr::airlib::ImageCaptureBase::ImageResponse> ResponseMap;
    };

}} // Namespace