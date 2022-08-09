using UnityEngine;
using System.Collections;

public class PathNode : NodeBase<PathNode>, IHeapItem<PathNode>
{
    int heapIndex;

    public int HeapIndex { get; set; }
    public bool Walkable { get; private set; }
    public Vector3 WorldPosition { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }

    public int Penalty { get; set; }

    public PathNode(bool walkable, Vector3 worldPosition, int x, int y, int penalty)
    {
        Walkable = walkable;
        WorldPosition = worldPosition;
        X = x;
        Y = y;
        Penalty = penalty;
    }

    public int CompareTo(PathNode nodeToCompare)
    {
        int compare = F.CompareTo(nodeToCompare.F);

        if (compare == 0)
            compare = H.CompareTo(nodeToCompare.H);

        return -compare;
    }

    public static int GetDistance(PathNode nodeA, PathNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.X - nodeB.X);
        int dstY = Mathf.Abs(nodeA.Y - nodeB.Y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);

        return 14 * dstX + 10 * (dstY - dstX);
    }

    public override int GetDistanceTo(PathNode node)
    {
        return GetDistance(this, node);
    }
}