import msgpackrpc #install as admin: pip install msgpack-rpc-python
import numpy as np #pip install numpy
import msgpack

from .utils import *
from .types import *

class PedestrianClient:
    def __init__(self, ip = "", port = 41452, timeout_value = 3600):
        if (ip == ""):
            ip = "127.0.0.1"
        self.client = msgpackrpc.Client(msgpackrpc.Address(ip, port), timeout = timeout_value, pack_encoding = 'utf-8', unpack_encoding = 'utf-8')

    # -----------------------------------  Common vehicle APIs ---------------------------------------------
    def reset(self, pedestrian_name = ''):
        """
        Reset the vehicle to its original starting state

        Note that you must call `enableApiControl` and `armDisarm` again after the call to reset
        """
        self.client.call('PedestrianReset', pedestrian_name)

    def ping(self):
        """
        If connection is established then this call will return true otherwise it will be blocked until timeout

        Returns:
            bool:
        """
        return self.client.call('PedestrianPing')

    def getClientVersion(self):
        return 1 # sync with C++ client

    def getServerVersion(self):
        return self.client.call('getServerVersion')

    def setPedestrianPose(self, pose, ignore_collison, pedestrian_name):
        """
        Set the pose of the vehicle

        If you don't want to change position (or orientation) then just set components of position (or orientation) to floating point nan values

        Args:
            pose (Pose): Desired Pose pf the vehicle
            ignore_collision (bool): Whether to ignore any collision or not
            pedestrian_name (str, optional): Name of the vehicle to move
        """
        self.client.call('SetPedestrianPose', pose, ignore_collison, pedestrian_name)

    def getPedestianPose(self, pedestrian_name):
        """
        Args:
            pedestrian_name (str, optional): Name of the vehicle to get the Pose of

        Returns:
            Pose:
        """
        pose = self.client.call('GetPedestrianPose', pedestrian_name)
        return Pose.from_msgpack(pose)

    def enableApiControl(self, is_enabled, pedestrian_name = ''):
        """
        Enables or disables API control for vehicle corresponding to pedestrian_name

        Args:
            is_enabled (bool): True to enable, False to disable API control
            pedestrian_name (str, optional): Name of the vehicle to send this command to
        """
        return self.client.call('PedestrianEnableApiControl', is_enabled, pedestrian_name)


    def setPedestrianControl(self, controls, pedestrian_name = ''):
        """
        Control the car using throttle, steering, brake, etc.

        Args:
            controls (PedestrianControls): Struct containing control values
            vehicle_name (str, optional): Name of vehicle to be controlled
        """
        self.client.call('setPedestrianControls', controls, pedestrian_name)


    # Pedestrian camera


    # Get a list of all pedestrians

