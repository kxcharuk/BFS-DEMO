using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{

    [SerializeField] public Waypoint startWaypoint;
    [SerializeField] public Waypoint endWaypoint;

    Waypoint searchCenter;

    List<Waypoint> path = new List<Waypoint>();
    Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();
    Queue<Waypoint> queue = new Queue<Waypoint>();

    private bool pathFound = false;


    Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    private void Awake()
    {
        startWaypoint.isStartWaypoint = true;
        endWaypoint.isEndWaypoint = true;
    }

    public List<Waypoint> GetPath()
    {
        if(path.Count == 0)
        {
            AddWaypointsToDictionary();
            BFS();
            CreatePath();
        }
        ChangeColorOfTilesInPath();
        return path;
    }

    private void AddWaypointsToDictionary()
    {
        Waypoint[] waypoints = FindObjectsOfType<Waypoint>();
        foreach(Waypoint waypoint in waypoints)
        {
            if(waypoint.isObstacle == false)
            {
                grid.Add(waypoint.GetGridPos(), waypoint);
            }
        }
    }

    // ------------------------------------------------------------------- BFS functions

    private void BFS()
    {
        queue.Enqueue(startWaypoint);
        while(queue.Count > 0 && !pathFound)
        {
            if(searchCenter == endWaypoint)
            {
                pathFound = true;
            }
            else
            {
                searchCenter = queue.Dequeue();
                SearchNeighbors();
                searchCenter.isExplored = true;
            }
        }
    }

    private void SearchNeighbors()
    {
        foreach(Vector2Int direction in directions)
        {
            Vector2Int searchCoords = searchCenter.GetGridPos() + direction;
            if (grid.ContainsKey(searchCoords))
            {
                QueueNeighbors(searchCoords);
            }
        }
    }

    private void QueueNeighbors(Vector2Int searchCoords)
    {
        Waypoint neighbor = grid[searchCoords];
        if(neighbor.isExplored || queue.Contains(neighbor))
        {
            return;
        }
        else
        {
            queue.Enqueue(neighbor);
            neighbor.searchedFrom = searchCenter; 
        }
    }

    // ------------------------------------------------------------------------------------ Creating the path

    
    private void CreatePath()
    {
        AddToPath(endWaypoint);
        Waypoint previousWaypoint = endWaypoint.searchedFrom;
        while(previousWaypoint != startWaypoint)
        {
            AddToPath(previousWaypoint);
            previousWaypoint = previousWaypoint.searchedFrom;
        }
        path.Add(startWaypoint);
        path.Reverse();
    }

    private void AddToPath(Waypoint waypoint)
    {
        path.Add(waypoint);
    }

    // ------------------------------------------------------------------- Change color of tiles for demo illustration
    private void ChangeColorOfTilesInPath()
    {
        foreach (Waypoint waypoint in path)
        {
            waypoint.GetComponentInChildren<MeshRenderer>().material.color = Color.magenta;
        }
        startWaypoint.GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
        endWaypoint.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
    }
}
