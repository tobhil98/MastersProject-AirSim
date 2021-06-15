import msgpackrpc #install as admin: pip install msgpack-rpc-python
import numpy as np #pip install numpy
import msgpack

from .utils import *
from .types import *

class AirSimServer:
    def __init__(self, ip = "", port = 41450, timeout_value = 3600):
        if (ip == ""):
            ip = "127.0.0.1"
        self.client = msgpackrpc.Client(msgpackrpc.Address(ip, port), timeout = timeout_value, pack_encoding = 'utf-8', unpack_encoding = 'utf-8')

    # -----------------------------------  Common vehicle APIs ---------------------------------------------
    def reset(self):
        """
        Reset the vehicle to its original starting state

        Note that you must call `enableApiControl` and `armDisarm` again after the call to reset
        """
        self.client.call('reset')

    def ping(self):
        """
        If connection is established then this call will return true otherwise it will be blocked until timeout

        Returns:
            bool:
        """
        return self.client.call('ServerPing')

    def getClientVersion(self):
        return 1 # sync with C++ client

    def getServerVersion(self):
        return self.client.call('getServerVersion')


    # Pause sim

    # Start sim

    # Restart sim


