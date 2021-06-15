import setup_path 
import airsim

#from keras.models import load_model
import sys
import numpy as np
import cv2
from time import sleep
import matplotlib.pyplot as plt
#if (len(sys.argv) != 2):
#    print('usage: python drive.py <modelName>')
#    sys.exit()

#print('Loading model...')
#model = load_model(sys.argv[1])

# connect to the AirSim simulator 
client = airsim.CarClient()
client.confirmConnection()
client.enableApiControl(True)
car_controls = airsim.CarControls()

car_controls.steering = 0
car_controls.throttle = 0
car_controls.brake = 0

image_buf = np.zeros((144, 2*256, 3))
state_buf = np.zeros((1,4))

cv2.imshow('image', image_buf)
sleep(1)


def get_image(cameraName = "0"):
    image = client.simGetImages([airsim.ImageRequest(cameraName, airsim.ImageType.Scene, False, False)],"PhysXCar")[0]
    image1d = np.fromstring(image.image_data_uint8, dtype=np.uint8)
    image_rgb = image1d.reshape(image.height, image.width, 3)[::-1,::]
    #gray = cv2.cvtColor(image_rgb, cv2.COLOR_BGR2GRAY)
    
    return image_rgb

def showImages(img1, img2):
    image = np.concatenate((img1,img2), axis=1)

    # stereo = cv2.StereoSGBM_create(numDisparities=128, blockSize=5)
    # stereo.setMode(3)
    # disparity = stereo.compute(img1, img2)

    cv2.imshow('image', image)
   # cv2.imshow('disp', disparity)
    cv2.waitKey(100)
    #plt.show()

    print("Got image: ", image.shape, image.shape, image.shape)

while (True):
    car_state = client.getCarState("PhysXCar")
   
    print('car speed: {0}'.format(car_state.speed))
    
    if (car_state.speed < 20):
        car_controls.throttle = 1.0
    else:
        car_controls.throttle = 0.0
    

    img1 = get_image("Front Left")
    img2 = get_image("1")
   # img3 = get_image("Front Left")
   # blank_image = np.zeros((144, 256, 3))

   # c2 = np.concatenate((img3,blank_image), axis=1)
   # image = np.concatenate((c1,c2), axis=2)

    showImages(img1.copy(), img2.copy())        
    #state_buf[0] = np.array([car_controls.steering, car_controls.throttle, car_controls.brake, car_state.speed])
    #model_output = model.predict([image_buf, state_buf])
    #car_controls.steering = float(model_output[0][0])
    car_controls.steering = 0
    
    print('Sending steering = {0}, throttle = {1}'.format(car_controls.steering, car_controls.throttle))
    
    client.setCarControls(car_controls, "PhysXCar")
