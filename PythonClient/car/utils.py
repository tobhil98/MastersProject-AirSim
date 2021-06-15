
import setup_path 
import airsim
import numpy as np
import asyncio


async def get_image(client, cameraName, vehicle_name):
    #print("Request", vehicle_name)
    image = client.simGetImages([airsim.ImageRequest(cameraName, airsim.ImageType.Scene, False, False)],vehicle_name)[0]
    #print(type(image))
    print("Image return", image.height)
    image1d = np.fromstring(image.image_data_uint8, dtype=np.uint8)
    image_rgb = image1d.reshape(image.height, image.width, 3)[::-1,::]
    return image_rgb

async def get_all_images(client, lst):
    tasks = []
    output = {}
    for (a,b) in lst:
        tasks.append(((a,b), asyncio.create_task(get_image(client,a,b))))
    for (i, c) in tasks:
        t = await c
        #print("Done", i[1])
        output[i] = t
    return output

def actually_get(client, lst):
    return asyncio.run(get_all_images(client, lst))

