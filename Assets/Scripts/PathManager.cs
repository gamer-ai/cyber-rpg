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

    public List<Vector2Int> FindCurrentPath(Vector3 worldStart, Vector3 worldTarget)
    {
        Vector2Int? start = world.WorldPosToGridPos(worldStart);
        Vector2Int? target = world.WorldPosToGridPos(worldTarget);
        if (start == null || target == null)
        {
            Debug.LogErrorFormat("Failed to convert world position {0} or " +
                "{1} to grid cell position ", worldStart, worldTarget);
            return null;
        }
        Debug.LogFormat("worldpos: {0}, cellpos: {1}", worldStart, start);
        Debug.LogFormat("worldpos: {0}, cellpos: {1}", worldTarget, target);
        List<PathFind.Point> points =
            PathFind.Pathfinding.FindPath(world.CurrentGrid.Value,
            new PathFind.Point(start.Value.x, start.Value.y),
            new PathFind.Point(target.Value.x, target.Value.y));
        List<Vector2Int> path = new List<Vector2Int>();
        foreach (PathFind.Point point in points)
        {
            path.Add(new Vector2Int(point.x, point.y));
        }
        return path;
    }

    public World world;
    private static PathManager instance_;
}
