import setup_path 
import airsim

import pprint
import time
import cv2
import numpy as np

# connect to the AirSim simulator 
client = airsim.CarClient()
client.confirmConnection()

pose = airsim.Pose(airsim.Vector3r(0, 0, 0), airsim.to_quaternion(0, 0, 0))
client.simAddVehicle("Test1", "PhysXCar", pose)

time.sleep(3)
print("API attempt")
#client.enableApiControl(True, "Test0")
client.enableApiControl(True, "Test1")
car_controls = airsim.CarControls()

#client.reset()

client.simPrintLogMessage("Hello", "645", 2)
client.simPrintTest("This is the important test")
print("Ready to try")
# go forward
car_controls.throttle = 0.5
car_controls.steering = 0
client.setCarControls(car_controls, "PhysXCar", "Test1")
print("Done")

def get_image(cameraName, vehicle_name):
    image = client.simGetImages([airsim.ImageRequest(cameraName, airsim.ImageType.Scene, False, False)],vehicle_name)[0]
    image1d = np.fromstring(image.image_data_uint8, dtype=np.uint8)
    image_rgb = image1d.reshape(image.height, image.width, 3)[::-1,::]
    return image_rgb

time.sleep(2)
client.simAddVehicle("Test0", "PhysXCar", pose)
time.sleep(1)
client.enableApiControl(True, "Test0")
car_controls.throttle = 1
car_controls.steering = 0.2
client.setCarControls(car_controls, "PhysXCar", "Test0")

while True:
    img1 = get_image("FC", "Test0")
    img2 = get_image("FC", "Test1")
    
    image = np.concatenate((img1, img2), axis=1)
    
    cv2.imshow('image', image)
    cv2.waitKey(20)



'''
while True:
    # get state of the car
    car_state = client.getCarState()
    print("Speed %d, Gear %d" % (car_state.speed, car_state.gear))

    collision_info = client.simGetCollisionInfo()

    if collision_info.has_collided:
        print("Collision at pos %s, normal %s, impact pt %s, penetration %f, name %s, obj id %d" % (
            pprint.pformat(collision_info.position), 
            pprint.pformat(collision_info.normal), 
            pprint.pformat(collision_info.impact_point), 
            collision_info.penetration_depth, collision_info.object_name, collision_info.object_id))
        break

    time.sleep(0.1)
'''
#client.enableApiControl(False)
