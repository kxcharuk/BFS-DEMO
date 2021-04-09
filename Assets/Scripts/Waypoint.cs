using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    //[HideInInspector] public bool hasTower = false;
    [HideInInspector] public bool isExplored = false;
    [SerializeField] public bool isObstacle = false; // not a path tile or a placeable tile
    private Vector2Int gridPos;
    [SerializeField] public Waypoint searchedFrom;

    private void Start()
    {

    }

    public Vector2Int GetGridPos()
    {
        return new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
        }
    }


}
