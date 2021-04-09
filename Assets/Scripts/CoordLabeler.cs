using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class CoordLabeler : MonoBehaviour
{

    private Vector2Int coords;

    // Start is called before the first frame update
    void Start()
    {
        coords = new Vector2Int();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoords();
        }
    }

    private void DisplayCoords()
    {
        coords.x = Mathf.RoundToInt(this.transform.position.x);
        coords.y = Mathf.RoundToInt(this.transform.position.z);

        gameObject.name = coords.x.ToString() + "," + coords.y.ToString();
    }
}
