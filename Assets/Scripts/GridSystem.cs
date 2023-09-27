using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public static GridSystem s_Instance;

    [SerializeField] public LayerMasks m_LayerMasks;

    [SerializeField] private int m_GridWidth;
    [SerializeField] private int m_GridHeight;

    [SerializeField] private GameObject m_GridCellPrefab;

    public Action<GridCellHoveredArgs> a_GridCellHovered;
    public Action<GridCellOccupiedArgs> a_GridCellOccupied;

    private GridCell[,] m_GridCells;
    private List<GridCell> m_GridCellList = new List<GridCell>();

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
        }
        else
        {
            Destroy(this);
        }

        m_GridCells = new GridCell[m_GridWidth, m_GridHeight];
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < m_GridWidth; x++)
        {
            for (int z = 0; z < m_GridHeight; z++)
            {
                GameObject gridCell = Instantiate(m_GridCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
                gridCell.transform.parent = transform;

                gridCell.GetComponent<GridCell>().Initialise(new Vector2Int(x, z));
                m_GridCells[x, z] = gridCell.GetComponent<GridCell>();

                m_GridCellList.Add(gridCell.GetComponent<GridCell>());
            }
        }
    }

    public static Vector3 Get2DDistanceAsVector(Vector3 a, Vector3 b)
    {
        Vector3 distance = new Vector3(Mathf.Abs(a.x - b.x), 0, Mathf.Abs(a.z - b.z));
        return distance;
    }

    public static float Get2DDistanceAsFloat(Vector3 a, Vector3 b)
    {
        return Get2DDistanceAsVector(a, b).magnitude;
    }

    public List<Vector2Int> FindPath(Vector2Int startCell, Vector2Int endCell)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        queue.Enqueue(startCell);
        cameFrom[startCell] = startCell;

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if (current == endCell)
            {
                // Reconstruct the path
                while (current != startCell)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }
                path.Reverse();
                return path;
            }

            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                if (!cameFrom.ContainsKey(neighbor) && !IsCellOccupied(neighbor))
                {
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        // No path found
        return path;
    }

    public List<Vector2Int> FindCellsInRange(Vector2Int startCell, int range)
    {
        List<Vector2Int> cellsInRange = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(startCell);
        visited.Add(startCell);

        while (queue.Count > 0 && range >= 0)
        {
            int levelSize = queue.Count;

            for (int i = 0; i < levelSize; i++)
            {
                Vector2Int current = queue.Dequeue();
                cellsInRange.Add(current);

                foreach (Vector2Int neighbor in GetNeighbors(current))
                {
                    if (!visited.Contains(neighbor) && !IsCellOccupied(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }


            range--;
        }

        return cellsInRange;
    }

    public void MarkCellsInRange(List<Vector2Int> cells)
    {
        List<GridCell> inRangeCells = new List<GridCell>();

        foreach (var cell in cells)
        {
            inRangeCells.Add(GetCellAtPos(new Vector3(cell.x, 0, cell.y)));
        }

        foreach (var cell in inRangeCells)
        {
            cell.MarkInRange();
        }

        m_GridCellList.Where(item => !inRangeCells.Contains(item)).ToList().ForEach(item => item.HideCell());
    }

    private List<Vector2Int> GetNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        if (cell.x > 0) neighbors.Add(new Vector2Int(cell.x - 1, cell.y));
        if (cell.x < m_GridWidth - 1) neighbors.Add(new Vector2Int(cell.x + 1, cell.y));
        if (cell.y > 0) neighbors.Add(new Vector2Int(cell.x, cell.y - 1));
        if (cell.y < m_GridHeight - 1) neighbors.Add(new Vector2Int(cell.x, cell.y + 1));
        return neighbors;
    }

    public void HighlightPath(List<Vector2Int> path)
    {
        foreach (Vector2Int cell in path)
        {
            m_GridCells[cell.x, cell.y].Highlight();
        }
    }

    public bool IsCellOccupied(Vector2Int cellPosition)
    {
        // Check if the cell is out of bounds
        if (cellPosition.x < 0 || cellPosition.x >= m_GridWidth || cellPosition.y < 0 || cellPosition.y >= m_GridHeight)
        {
            return false;
        }

        // Get the GridCell at the specified position
        GridCell cell = m_GridCells[cellPosition.x, cellPosition.y].GetComponent<GridCell>();

        // Check if the cell is occupied by a player unit
        return cell.IsOccupied();
    }

    public GridCell GetCellAtPos(Vector3 position)
    {
        // Check if the cell is out of bounds
        if (position.x < 0 || position.x >= m_GridWidth || position.z < 0 || position.z >= m_GridHeight)
        {
            return null;
        }

        // Get the GridCell at the specified position
        return m_GridCells[(int)position.x, (int)position.z];
    }

    public struct GridCellHoveredArgs
    {
        public GridCell m_HoveredGridCell;
    }

    public struct GridCellOccupiedArgs
    {
        public GridCell m_OccupiedGridCell;
        public PlayerUnit m_OccupyingPlayerUnit;
    }
}
