from time import sleep

import setup_path 
import airsim

client = airsim.CarClient(port=41450)
status = client.ping()
print(status)
#client.simPrintTest("This is the important test")
pose = airsim.Pose(airsim.Vector3r(0, 0, 0), airsim.to_quaternion(0, 0, 0))
client.simAddVehicle("Test", "PhysXCar", pose)
sleep(2)
client.simRemoveVehicle("Test", "PhysXCar")