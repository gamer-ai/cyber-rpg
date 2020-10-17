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
        List<PathFind.Point> points =
            PathFind.Pathfinding.FindPath(world.CurrentGrid.Value,
            new PathFind.Point(0, 0), new PathFind.Point(10, 10));
        foreach (var point in points)
        {
            Debug.LogFormat("{0}, {1}", point.x, point.y);
        }
    }

    public List<Vector2> FindCurrentPath(Vector2 worldStart, Vector2 wordTarget)
    {
        return null;
    }

    public World world;
    private static PathManager instance_;
}
