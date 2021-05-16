using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirSimUnity
{
    public class AddPedestrain : MonoBehaviour
    {
        // Start is called before the first frame update
        private static AddPedestrain instance;
        public static AddPedestrain GetInstance()
        {
            return instance;
        }
        private void Awake()
        {
            instance = this;
        }

        private struct InitPedestrain
        {
            public string name;
            public Vector3 pos;
            public Quaternion rotation;
        }
        private static int counter = 0;

        private static Queue<InitPedestrain> PedestrainQueue = new Queue<InitPedestrain>();
        private void Update()
        {
            if (PedestrainQueue.Count > 0)
            {
                InitPedestrain s = PedestrainQueue.Dequeue();
                var obj = Instantiate(AssetHandler.getInstance().getPedestrian(), s.pos, s.rotation);
                AirSimServer.pedestrianList.Add(obj);
                var component = obj.GetComponent<PedestrianOverhead>();
                component.name = s.name;
            }
        }

        async public void Pressed()
        {
            Debug.LogWarning("Adding new pedestrian");
            var s = new InitPedestrain();
            s.name = "Tim" + ++counter;
            s.pos = new Vector3(-225, 2, 50);
            s.rotation = Quaternion.identity;
            PedestrainQueue.Enqueue(s);
        }

        async public void SpawnPedestrian(string name, Vector3 pos, Quaternion rotation)
        {
            var s = new InitPedestrain();
            s.name = name;
            s.pos = pos;
            s.rotation = rotation;
            PedestrainQueue.Enqueue(s);
        }

    }
}
