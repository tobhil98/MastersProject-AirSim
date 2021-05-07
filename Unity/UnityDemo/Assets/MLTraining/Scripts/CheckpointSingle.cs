using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AirSimUnity;

public class CheckpointSingle : MonoBehaviour
{

    private TrackCheckpoints trackCheckpoints;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.LogError("Car");
        //Debug.LogError(other.GetComponent<CarCollision>());
        if (other.TryGetComponent<CarCollision>(out CarCollision car))
        {
            trackCheckpoints.CarThroughCheckpoint(this, other.transform);
        }
    }

    public void SetTrackCheckpoints(TrackCheckpoints trackCheckpoints)
    {
        this.trackCheckpoints = trackCheckpoints;
    }
}
