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
            for (int y = 0; y < m_GridHeight; y++)
            {
                GameObject gridCell = Instantiate(m_GridCellPrefab, new Vector3(x, 0, y), Quaternion.identity);
                gridCell.transform.parent = transform;

                gridCell.GetComponent<GridCell>().Initialise(new Vector2Int(x, y));
                m_GridCells[x, y] = gridCell;
            }
        }   
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
