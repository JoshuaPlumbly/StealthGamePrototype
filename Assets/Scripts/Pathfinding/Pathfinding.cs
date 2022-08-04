using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

public class Pathfinding
{
    Grid _grid;

    public Pathfinding(Grid grid)
    {
        _grid = grid;
    }

    public void FindPath(PathRequest request, Action<PathResult> callback)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = _grid.NodeFromWorldPoint(request.PathStart);
        Node targetNode = _grid.NodeFromWorldPoint(request.PathEnd);
        startNode.Parent = startNode;


        if (startNode.Walkable && targetNode.Walkable)
        {
            Heap<Node> openSet = new Heap<Node>(_grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                // Detect if a path to the target node has been found.
                if (currentNode == targetNode)
                {
                    sw.Stop();
                    //print ("Path found: " + sw.ElapsedMilliseconds + " ms");
                    pathSuccess = true;
                    break;
                }

                // Check neighbouring nodes.
                foreach (Node neighbour in _grid.GetNeighbours(currentNode))
                {
                    // Move on if neighbour is not walkable or already checked.
                    if (!neighbour.Walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour) + neighbour.Penalty;

                    if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }
        }

        // Retrace path when a path has been found.
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            pathSuccess = waypoints.Length > 0;
        }

        // Run callback.
        callback(new PathResult(waypoints, pathSuccess, request.Callback));
    }


    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            // Add node to path and move on to the parent.
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        // Simplify path and reverse.
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);

        return waypoints;

    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            // Find the direction between previous and current waypoints.
            Vector2 directionNew = new Vector2(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);

            // Add waypoint to a list when a new direction is detected.
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].WorldPosition);
            }

            // Remember the direction between previous and current waypoint.
            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);

        return 14 * dstX + 10 * (dstY - dstX);
    }
}