using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    WaypointManager waypointManager;
    
    [HideInInspector] public bool isExplored = false;
    [SerializeField] public bool isObstacle = false; // not a path tile or a placeable tile

    [HideInInspector] public bool isStartWaypoint = false;
    [HideInInspector] public bool isEndWaypoint = false;

    /*[HideInInspector] public bool isSelected = false;
    [HideInInspector] public bool canSelect = true;*/

    private Vector2Int gridPos;
    [SerializeField] public Waypoint searchedFrom;

    private MeshRenderer meshRenderer;

    private void Start()
    {
        waypointManager = FindObjectOfType<WaypointManager>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        SetColorAtStart();
    }

    private void Update()
    {
        if (isStartWaypoint)
        {
            meshRenderer.material.color = Color.green;
        }
        else if (isEndWaypoint)
        {
            meshRenderer.material.color = Color.red;
        }
    }

    public Vector2Int GetGridPos()
    {
        return new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
    }

    private void SetColorAtStart()
    {
        if (isStartWaypoint)
        {
            meshRenderer.material.color = Color.green;
        }
        else if (isEndWaypoint)
        {
            meshRenderer.material.color = Color.red;
        }
        else
        {
            meshRenderer.material.color = Color.gray;
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(waypointManager.waypoints.Count == 0)// only want to add if the queue is empty, otherwise we are swapping pos
            {
                if(isStartWaypoint || isEndWaypoint)
                {
                    waypointManager.waypoints.Enqueue(this);
                    waypointManager.lastQueuedWaypointPos = this.gameObject.transform.position;
                }
            }
            else
            {
                //Vector3 newPos = new Vector3();
                //newPos = waypointManager.waypoints.Dequeue().transform.position;
                //this.transform.position = newPos;
                //waypointManager.lastQueuedWaypointPos = this.transform.position;
                waypointManager.waypoints.Dequeue().transform.position = this.transform.position;
                this.transform.position = waypointManager.lastQueuedWaypointPos;
                
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (isObstacle)
            {
                meshRenderer.material.color = Color.gray;
                isObstacle = false;
            }
            else
            {
                meshRenderer.material.color = Color.black;
                isObstacle = true;
            }
        }
    }


}
