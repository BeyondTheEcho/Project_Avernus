using System;
using System.Collections;
using System.Collections.Generic;
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
    public Action<MoveActionArgs> a_MoveAction;

    private GameObject[,] m_GridCells;

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

        m_GridCells = new GameObject[m_GridWidth, m_GridHeight];
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
                m_GridCells[x, z] = gridCell;
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

    public struct MoveActionArgs
    {
        public UnitBase m_Unit;
        public bool m_HighlightOnlyValidMoves;
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
