using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node>
{

    bool walkable;
    Vector3 worldPosition;
    int gridX;
    int gridY;
    int penalty;

    int gCost;
    int hCost;
    Node parent;
    int heapIndex;

    #region Encapsulated variables
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public bool Walkable
    {
        get
        {
            return walkable;
        }

        set
        {
            walkable = value;
        }
    }

    public Vector3 WorldPosition
    {
        get
        {
            return worldPosition;
        }

        set
        {
            worldPosition = value;
        }
    }

    public int GridX
    {
        get
        {
            return gridX;
        }

        set
        {
            gridX = value;
        }
    }

    public int GridY
    {
        get
        {
            return gridY;
        }

        set
        {
            gridY = value;
        }
    }

    public int Penalty
    {
        get
        {
            return penalty;
        }

        set
        {
            penalty = value;
        }
    }

    public int GCost
    {
        get
        {
            return gCost;
        }

        set
        {
            gCost = value;
        }
    }

    public int HCost
    {
        get
        {
            return hCost;
        }

        set
        {
            hCost = value;
        }
    }

    public Node Parent
    {
        get
        {
            return parent;
        }

        set
        {
            parent = value;
        }
    }
    #endregion

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY, int _penalty)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        penalty = _penalty;
    }

    public int FCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
