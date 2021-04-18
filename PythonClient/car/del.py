from time import sleep

import setup_path 
import airsim

client = airsim.CarClient(port=41450)
status = client.ping()
print(status)
client.simPrintTest("This is the important test")
