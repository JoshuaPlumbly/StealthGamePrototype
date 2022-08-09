using UnityEngine;
using System;

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
