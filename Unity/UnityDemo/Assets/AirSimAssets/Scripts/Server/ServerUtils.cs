using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;


namespace AirSimUnity
{

    namespace ServerUtils
    {

        [StructLayout(LayoutKind.Sequential)]
        public struct VehicleTypes {
            public int length;
            public int elements;
            public char[] str;

            public void Reset()
            {
                length = 0;
                elements = 0;
                str = null;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CurrentVehicles
        {
        }



    }

}
