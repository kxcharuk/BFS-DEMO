using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{

    [HideInInspector] public Queue<Waypoint> waypoints = new Queue<Waypoint>();
    [HideInInspector] public Vector3 lastQueuedWaypointPos; // changed to vector3 -> make appropriate changes in waypoint script

    void Awake()
    {
        
    }
    


}
