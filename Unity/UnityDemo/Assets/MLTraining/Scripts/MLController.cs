using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public static class Extension
{
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}

public class MLController : MonoBehaviour
{

    [Header("Max Environment Steps")] public int MaxEnvironmentSteps = 2000;

    public int count = 0;
    public Transform car;
    public Transform objects;

    private SimpleMultiAgentGroup m_AgentGroup;
    private List<MLCar> carLst;
    private List<Transform> goals;

    public void Awake()
    {

        goals = new List<Transform>();
        foreach (Transform i in objects)
        {
            goals.Add(i);
        }

        Debug.Assert(goals.Count > 2);

        if (count > goals.Count)
        {
            count = goals.Count;
        }






        ResetScene();
    }

    void ResetScene()
    {
        m_ResetTimer = 0;

        carLst = new List<MLCar>();
        m_AgentGroup = new SimpleMultiAgentGroup();


        for (int i = 0; i < count; i++)
        {
            var t = Instantiate(car);
            t.parent = transform.parent;
            var a = t.GetComponent<MLCar>();
            carLst.Add(a);
            a.Setup(this, i);
            m_AgentGroup.RegisterAgent(a);
        }
        //Debug.Log(m_AgentGroup.GetRegisteredAgents().Count);

        Extension.Shuffle(goals);

        int index = 0;
        foreach(var item in carLst)
        {
            // Random target
            int target = 0;
            do
            {
                target = Random.Range(0, goals.Count);
            } while (target == index);

            //item.selfTransfrom.SetPositionAndRotation(goals[index].position, goals[index].rotation);
            item.SetPosition(goals[index].position, goals[index].rotation);

            item.target = goals[target];
            Debug.Log("Player" + index + " - " + goals[index].name + ", " + goals[target].name);

            index++;
        }

        activeCars = count;

    }

    public void CarFinished(MLCar carId)
    {
        m_AgentGroup.AddGroupReward(1f);
        CarDisable(carId);
    }

    public void CarCollision()
    {
        m_AgentGroup.AddGroupReward(-1f);
        m_AgentGroup.EndGroupEpisode();
        Cleanup();
        ResetScene();
    }

    public void CarDisable(MLCar carID, bool list = false)
    {
        if (!list)
        {
            carLst.Remove(carID);
        }

        activeCars--;
        carID.Kill();

    }

    private void Cleanup()
    {
        foreach (var i in carLst)
        {
            CarDisable(i, true);
        }
    }


    private int activeCars;

    private int m_ResetTimer;
    private void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            Debug.Log("Timeout!");
            Cleanup();
            m_AgentGroup.GroupEpisodeInterrupted();
            ResetScene();
        }

        if (activeCars == 0) {
            m_AgentGroup.AddGroupReward(1 - m_ResetTimer/MaxEnvironmentSteps);
            m_AgentGroup.EndGroupEpisode();
            ResetScene();
        }

    }




    /*    public Transform car;
        public Transform target;
        public Transform carPrefab;

        private Vector3 carInitPos;

        private Transform parent;

        public void Start()
        {
            carInitPos = car.position;

            parent = car.parent;
        }


        public void resetMap()
        {
            Transform t = Instantiate(carPrefab);
            t.parent = parent;
            t.position = carInitPos;
            t.GetComponent<MLCar>().targetTransform = target;
            Debug.LogError("MapReset");
        }*/

}


