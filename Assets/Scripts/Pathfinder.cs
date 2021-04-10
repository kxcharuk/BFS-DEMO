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

    Waypoint[] wayp;

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

    public /*List<Waypoint>*/void GetPath()
    {
        ResetPathfinder();
        if(path.Count == 0)
        {
            AddWaypointsToDictionary();
            BFS();
            CreatePath();
        }
        ChangeColorOfTilesInPath();
        //return path;
    }

    private void ResetPathfinder()
    {
        wayp = FindObjectsOfType<Waypoint>();
        ResetTileColors();
        path.Clear();
        grid.Clear();
        queue.Clear();
        searchCenter = null;
        foreach(Waypoint way in wayp)
        {
            way.isExplored = false;
        }
        pathFound = false;
    }

    private void ResetTileColors()
    {
        foreach (Waypoint way in wayp)
        {
            if (way.isStartWaypoint || way.isEndWaypoint || way.isObstacle)
            {

            }
            else
            {
                way.GetComponentInChildren<MeshRenderer>().material.color = Color.gray;
            }
        }
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
                //StartCoroutine(Search());
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
            if(neighbor.isEndWaypoint || neighbor.isStartWaypoint) { }
            else
            {
                neighbor.GetComponentInChildren<MeshRenderer>().material.color = Color.cyan;
            }
            queue.Enqueue(neighbor);
            neighbor.searchedFrom = searchCenter; 
        }
    }

    IEnumerator Search()
    {
        foreach (Vector2Int direction in directions)
        {
            Vector2Int searchCoords = searchCenter.GetGridPos() + direction;
            if (grid.ContainsKey(searchCoords))
            {
                //QueueNeighbors(searchCoords);
                StartCoroutine(Queue(searchCoords));
                
            }
            yield return new WaitForSeconds(4);
        }
    }

    IEnumerator Queue(Vector2Int searchCoords)
    {
        Waypoint neighbor = grid[searchCoords];
        if (neighbor.isExplored || queue.Contains(neighbor))
        {
            //yield return new WaitForSeconds(1);
            yield return new WaitForSeconds(0.1f);
        }
        else
        {
            if (neighbor.isEndWaypoint || neighbor.isStartWaypoint) { }
            else
            {
                neighbor.GetComponentInChildren<MeshRenderer>().material.color = Color.cyan;
            }
            queue.Enqueue(neighbor);
            neighbor.searchedFrom = searchCenter;
        }
        yield return new WaitForSeconds(0.1f);
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
            if(waypoint.isStartWaypoint || waypoint.isEndWaypoint)
            {

            }
            else
            {
                waypoint.GetComponentInChildren<MeshRenderer>().material.color = Color.magenta;
            }
        }
    }
}
