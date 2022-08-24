using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    public readonly Vector3[] _wayPoints;
    public readonly Line[] _turnBoundaries;
    public readonly int _finishLineIndex;
    public readonly int _slowDownIndex;

    public int WaypointCount => _wayPoints.Length;

    public Path(Vector3[] waypoints, Vector3 startPos, float turnDst, float stoppingDst)
    {
        _wayPoints = waypoints;
        _turnBoundaries = new Line[_wayPoints.Length];
        _finishLineIndex = _turnBoundaries.Length - 1;

        // Get the starting posion as a 2D vector.
        Vector2 previousPoint = V3ToV2(startPos);

        // Smooth the path between each waypoint.
        for (int i = 0; i < _wayPoints.Length; i++)
        {
            Vector2 currentPoint = V3ToV2(_wayPoints[i]);
            Vector2 dirToCurrentPoint = (currentPoint - previousPoint).normalized;
            Vector2 turnBoundaryPoint = (i == _finishLineIndex) ? currentPoint : currentPoint - dirToCurrentPoint * turnDst;
            _turnBoundaries[i] = new Line(turnBoundaryPoint, previousPoint - dirToCurrentPoint * turnDst);
            previousPoint = turnBoundaryPoint;
        }

        float dstFromEndPoint = 0;

        for (int i = _wayPoints.Length - 1; i > 0; i--)
        {
            dstFromEndPoint += Vector3.Distance(_wayPoints[i], _wayPoints[i - 1]);

            if (dstFromEndPoint > stoppingDst)
            {
                _slowDownIndex = i;
                break;
            }
        }
    }

    /// <summary>
    /// Convert Vector 3 to Vector 2.
    /// </summary>
    /// <param name="v3"></param>
    /// <returns></returns>
    public static Vector2 V3ToV2(Vector3 v3)
    {
        return new Vector2(v3.x, v3.z);
    }

    public void DrawWithGizmos()
    {
        Gizmos.color = Color.black;
        foreach (Vector3 p in _wayPoints)
        {
            Gizmos.DrawCube(p + Vector3.up, Vector3.one);
        }

        Gizmos.color = Color.white;
        foreach (Line l in _turnBoundaries)
        {
            l.DrawWithGizmos(10);
        }
    }
}