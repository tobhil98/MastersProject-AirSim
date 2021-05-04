from time import sleep

import setup_path 
import airsim
import utils

import numpy as np
import cv2

server = airsim.CarClient(port=41450)
client = airsim.CarClient(port=41451)
status = server.ping()
print(status)
server.simPrintTest("This is the important test")
response = server.simGetVehicleTypes()
print(response)

print(server.simGetAllVehiclesList())
print(server.simGetAllPedestriansList())


# pose = airsim.Pose(airsim.Vector3r(0, 0, 0), airsim.to_quaternion(0, 0, 0))
# server.simAddVehicle("Test3", response[1], pose)


# pose = airsim.Pose(airsim.Vector3r(0, 0, 0), airsim.to_quaternion(0, 0, 0))
# server.simAddVehicle("Test", "PhysXCar", pose)
# server.simAddPedestrian("TestPedestrian", pose)
# server.simAddPedestrian("TestPedestrian1", pose)
# sleep(2)
# pedClient = airsim.PedestrianClient(port=41452)
# print("Client created")
# print("Ped client ping : ", pedClient.ping())
# sleep(0.3)
# pedClient.enableApiControl(True, "TestPedestrian")
# #client.enableApiControl(True)
# pedestrian_controls = airsim.PedestrianControls()
# pedestrian_controls.speed = 0.3
# pedestrian_controls.steering = 0
# pedClient.setPedestrianControl(pedestrian_controls, "TestPedestrian")
# #print("reset ", pedClient.reset())
# # car_controls = airsim.CarControls()
# # car_controls.throttle = 0.5
# # car_controls.steering = 0
# # client.setCarControls(car_controls, "PhysXCar", "Test")

# print("Enable api control done")
# while True:
#     lst = [("Left", "TestPedestrian1"), ("Right", "TestPedestrian1")]
#     imgDict = utils.actually_get(pedClient, lst)

#     img1 = imgDict[("Left", "TestPedestrian1")]
#     #img2 = imgDict[("Right", "TestPedestrian1")]

#     lst2 = [("FC", "Test"), ("FC", "Test")]
#     imgDict = utils.actually_get(client, lst2)
    
#     #img1 = imgDict[("FC", "Test")]
#     img2 = imgDict[("FC", "Test")]
    
#     image = np.concatenate((img1, img2), axis=1)
#     cv2.imshow('image', image)
#     cv2.waitKey(20)
# # pedestrian_controls.speed = 0
# # pedestrian_controls.steering = 0
# # pedClient.setPedestrianControl(pedestrian_controls, "TestPedestrian")

# # car_controls.throttle = 0
# # car_controls.steering = 0
# # car_controls.handbrake = True
# # client.setCarControls(car_controls, "PhysXCar", "Test")

# server.simRemoveVehicle("Test", "PhysXCar")
# server.simRemovePedestrian("TestPedestrian")
# # for i in range(5):
# #     server.simAddVehicle("Test", "PhysXCar", pose)
# #     sleep(2)
# #     client = airsim.CarClient(port=41451)
# #     client.confirmConnection()
# #     client.enableApiControl(True, "Test")
# #     car_controls = airsim.CarControls()
# #     car_controls.throttle = 0.5
# #     car_controls.steering = 0
# #     client.setCarControls(car_controls, "PhysXCar", "Test")
# #     client.setCarControls(car_controls, "PhysXCar", "Test")
# #     sleep(5)
# #     server.simRemoveVehicle("Test", "PhysXCar")