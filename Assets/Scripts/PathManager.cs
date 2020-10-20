using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static PathManager Instance
    {
        get
        {
            if (instance_ == null)
            {
                instance_ = FindObjectOfType<PathManager>();
            }
            return instance_;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Vector3> FindCurrentPath(Vector3 worldStart, Vector3 worldTarget)
    {
        Vector2Int? start = world.WorldPosToGridPos(worldStart);
        Vector2Int? target = world.WorldPosToGridPos(worldTarget);
        if (start == null || target == null)
        {
            Debug.LogErrorFormat("Failed to convert world position {0} or " +
                "{1} to grid cell position ", worldStart, worldTarget);
            return null;
        }
        List<PathFind.Point> points =
            PathFind.Pathfinding.FindPath(world.CurrentGrid.Value,
            new PathFind.Point(start.Value.x, start.Value.y),
            new PathFind.Point(target.Value.x, target.Value.y));
        List<Vector3> path = new List<Vector3>();
        foreach (PathFind.Point point in points)
        {
            Vector2Int gridPos = new Vector2Int(point.x, point.y);
            Vector3? worldPoint = world.GridPosToWorldPos(gridPos);
            if (worldPoint == null)
            {
                Debug.LogErrorFormat("Failed to convert grid cell position {0} "
                    + "to world position.", gridPos);
                continue;
            }
            path.Add(worldPoint.Value);
        }
        return path;
    }

    public World world;
    private static PathManager instance_;
}
