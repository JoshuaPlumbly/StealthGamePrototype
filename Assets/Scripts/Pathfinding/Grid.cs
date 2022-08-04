
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] LayerMask unwalkableMask;
    [SerializeField] Vector2 gridWorldSize;
    [SerializeField] float nodeRadius;
    [SerializeField] Node[,] grid;
    [SerializeField] TerrainType[] walkableTerrain;
    [SerializeField] int obstacleProxPenalty = 10;

    [Header("Draw Gizmo")]
    [SerializeField]
    bool drawArea;
    [SerializeField] bool drawGrid;
    [SerializeField] bool drawPath;
    [SerializeField] Color areaColour = Color.blue;
    [SerializeField] Color lowWeightColour = Color.green;
    [SerializeField] Color highWeightColour = Color.red;
    [SerializeField] Color unwalkableColour = Color.red;
    [SerializeField] Color pathColour = Color.blue;

    LayerMask walkableMask;
    public Dictionary<int, int> walkableTerrainDictionary = new Dictionary<int, int>();

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    int penaltyMin = int.MaxValue;
    int penaltyMax = int.MinValue;

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        foreach (TerrainType region in walkableTerrain)
        {
            walkableMask.value |= region.terrainMask.value;
            walkableTerrainDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }

        CreateGrid();
    }

    /// <summary>
    /// Creates a grid of nodes.
    /// </summary>
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint =
                    worldBottomLeft +
                    Vector3.right * (x * nodeDiameter + nodeRadius) +
                    Vector3.forward * (y * nodeDiameter + nodeRadius);

                // Check object is not being blocked by an object.
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                // Create a penaly vaules of weights.
                int penalty = 0;

                // Add a weight penaly to a node based on it's layer mask.
                Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, walkableMask))
                {
                    walkableTerrainDictionary.TryGetValue(hit.collider.gameObject.layer, out penalty);
                }

                if (!walkable)
                {
                    penalty += obstacleProxPenalty;
                }

                // Add a new node to the grid.
                grid[x, y] = new Node(walkable, worldPoint, x, y, penalty);
            }

            //BlurPenaltyMap(3);
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        float percentX = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    /// <summary>
    /// Return a list of all neighbouring nodes.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public List<Node> GetNeighbours(Node n)
    {
        // Create a list for neighbouring nodes.
        List<Node> neighbours = new List<Node>();

        // Add neighbouring nodes to a list.
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // Exclude the node that was being checked. 
                if (x == 0 && y == 0)
                    continue;

                int checkX = n.GridX + x;
                int checkY = n.GridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        // Return list of neighbouring nodes.
        return neighbours;
    }

    void BlurPenaltyMap(int blurSize)
    {
        int kernelSize = blurSize * 2 + 1;
        int kernelExtents = (kernelSize - 1) / 2;

        int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
        int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = -kernelExtents; x <= kernelExtents; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                penaltiesHorizontalPass[0, y] += grid[sampleX, y].Penalty;
            }

            for (int x = 1; x < gridSizeX; x++)
            {
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);

                penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - grid[removeIndex, y].Penalty + grid[addIndex, y].Penalty;
            }
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = -kernelExtents; y <= kernelExtents; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, kernelExtents);
                penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
            }

            int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
            grid[x, 0].Penalty = blurredPenalty;

            for (int y = 1; y < gridSizeY; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);

                penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];
                blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
                grid[x, y].Penalty = blurredPenalty;

                if (blurredPenalty > penaltyMax)
                {
                    penaltyMax = blurredPenalty;
                }
                if (blurredPenalty < penaltyMin)
                {
                    penaltyMin = blurredPenalty;
                }
            }
        }
    }

    public List<Node> path;
    private void OnDrawGizmos()
    {

        // Draw grid size
        Gizmos.color = areaColour;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        // Draw grid nodes
        if (drawGrid && grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = Color.Lerp(lowWeightColour, highWeightColour, Mathf.InverseLerp(penaltyMin, penaltyMax, n.Penalty));

                if (!n.Walkable)
                    Gizmos.color = unwalkableColour;

                if (drawPath && path != null)
                {
                    if (!path.Contains(n))
                        Gizmos.color = pathColour;
                }

                Gizmos.DrawCube(n.WorldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}

[System.Serializable]
public class TerrainType
{
    public LayerMask terrainMask;
    public int terrainPenalty;
    public float footSteps;
}