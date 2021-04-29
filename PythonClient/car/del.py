from time import sleep

import setup_path 
import airsim

server = airsim.CarClient(port=41450)
client = airsim.CarClient(port=41451)
status = server.ping()
print(status)
server.simPrintTest("This is the important test")
pose = airsim.Pose(airsim.Vector3r(0, 0, 0), airsim.to_quaternion(0, 0, 0))
server.simAddVehicle("Test", "PhysXCar", pose)
server.simAddPedestrian("TestPedestrian", pose)
sleep(2)
pedClient = airsim.PedestrianClient(port=41452)
print("Client created")
print("Ped client ping : ", pedClient.ping())
sleep(0.3)
pedClient.enableApiControl(True)
client.enableApiControl(True)
pedestrian_controls = airsim.PedestrianControls()
pedestrian_controls.speed = 0.5
pedestrian_controls.steering = 0
pedClient.setPedestrianControl(pedestrian_controls, "TestPedestrian")
#print("reset ", pedClient.reset())
car_controls = airsim.CarControls()
car_controls.throttle = 0.5
car_controls.steering = 0
client.setCarControls(car_controls, "PhysXCar", "Test")

print("Enable api control done")
sleep(5)
server.simRemoveVehicle("Test", "PhysXCar")
server.simRemovePedestrian("TestPedestrian")
# for i in range(5):
#     server.simAddVehicle("Test", "PhysXCar", pose)
#     sleep(2)
#     client = airsim.CarClient(port=41451)
#     client.confirmConnection()
#     client.enableApiControl(True, "Test")
#     car_controls = airsim.CarControls()
#     car_controls.throttle = 0.5
#     car_controls.steering = 0
#     client.setCarControls(car_controls, "PhysXCar", "Test")
#     client.setCarControls(car_controls, "PhysXCar", "Test")
#     sleep(5)
#     server.simRemoveVehicle("Test", "PhysXCar")