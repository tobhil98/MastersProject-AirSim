import setup_path 
import airsim

import pprint
import time

# connect to the AirSim simulator 
client = airsim.CarClient()
client.confirmConnection()

pose = airsim.Pose(airsim.Vector3r(0, 0, 0), airsim.to_quaternion(0, 0, 0))
#client.simAddVehicle("Test0", "PhysXCar", pose)
client.simAddVehicle("Test1", "PhysXCar", pose)

time.sleep(3)
print("API attempt")
#client.enableApiControl(True, "Test0")
client.enableApiControl(True, "Test1")
car_controls = airsim.CarControls()

client.reset()

client.simPrintLogMessage("Hello", "645", 2)
client.simPrintTest("This is the important test")
print("Ready to try")
# go forward
car_controls.throttle = 0.5
car_controls.steering = 0
client.setCarControls(car_controls, "PhysXCar", "Test1")
print("Done")
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

#client.enableApiControl(False)
