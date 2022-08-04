using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

[RequireComponent(typeof(Grid))]
public class PathRequestManager : MonoBehaviour
{
    Queue<PathResult> results = new Queue<PathResult>();

    static PathRequestManager instance;
    Pathfinding pathfinding;

    void Awake()
    {
        instance = this;
        pathfinding = new Pathfinding(GetComponent<Grid>());
    }

    void Update()
    {
        if (results.Count > 0)
        {
            int itemsInQueue = results.Count;
            lock (results)
            {
                for (int i = 0; i < itemsInQueue; i++)
                {
                    PathResult result = results.Dequeue();
                    result.Callback(result.Path, result.Success);
                }
            }
        }
    }

    public static void RequestPath(PathRequest request)
    {
        ThreadStart threadStart = delegate
        {
            instance.pathfinding.FindPath(request, instance.FinishedProcessingPath);
        };
        threadStart.Invoke();
    }

    public void FinishedProcessingPath(PathResult result)
    {
        lock (results)
        {
            results.Enqueue(result);
        }
    }
}

public struct PathResult
{
    public Vector3[] Path;
    public bool Success;
    public Action<Vector3[], bool> Callback;

    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
    {
        this.Path = path;
        this.Success = success;
        this.Callback = callback;
    }
}

public struct PathRequest
{
    public Vector3 PathStart;
    public Vector3 PathEnd;
    public Action<Vector3[], bool> Callback;

    public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> callback)
    {
        PathStart = start;
        PathEnd = end;
        Callback = callback;
    }
}