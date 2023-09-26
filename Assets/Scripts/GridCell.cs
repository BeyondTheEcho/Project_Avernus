using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField] private Material m_DefaultMaterial;
    [SerializeField] private Material m_HoveredMaterial;
    [SerializeField] private Material m_OccupiedMaterial;
    [SerializeField] private TMP_Text m_GridCellText;
    [SerializeField] private MeshRenderer m_MeshRenderer;

    private Vector2Int m_GridPosition;
    private PlayerUnit m_OccupyingPlayerUnit;

    // Start is called before the first frame update
    void Awake()
    {
        GridSystem.s_Instance.a_GridCellHovered += OnHovered;
        GridSystem.s_Instance.a_GridCellOccupied += OnOccupied;
        GridSystem.s_Instance.a_MoveAction += OnMoveAction;
    }

    // Update is called once per frame
    void Update()
    {
        m_GridCellText.text = $"{m_GridPosition.x}, {m_GridPosition.y}";
    }

    public void Initialise(Vector2Int gridPosition)
    {
        m_GridPosition = gridPosition;
    }

    public void SetOccupyingUnit(PlayerUnit playerUnit)
    {
        m_OccupyingPlayerUnit = playerUnit;
        m_MeshRenderer.material = m_OccupiedMaterial;
    }

    public void ClearOccupyingUnit()
    {
        m_OccupyingPlayerUnit = null;
        m_MeshRenderer.material = m_DefaultMaterial;
    }

    public void OnMoveAction(GridSystem.MoveActionArgs args)
    {
        if (args.m_HighlightOnlyValidMoves && GridSystem.Get2DDistanceAsFloat(transform.position, args.m_Unit.transform.position) > args.m_Unit.GetRemainingMovementRange())
        {
            m_MeshRenderer.enabled = false;
        }
        else
        {
            m_MeshRenderer.enabled = true;
        }
    }

    public void OnHovered(GridSystem.GridCellHoveredArgs args)
    {
        if (m_OccupyingPlayerUnit != null) return;

        if (args.m_HoveredGridCell == this)
        {
            m_MeshRenderer.material = m_HoveredMaterial;
        }
        else
        {
            m_MeshRenderer.material = m_DefaultMaterial;
        }
    }

    public void OnOccupied(GridSystem.GridCellOccupiedArgs args)
    {
        if (args.m_OccupiedGridCell == this)
        {
            m_MeshRenderer.material = m_OccupiedMaterial;
            m_OccupyingPlayerUnit = args.m_OccupyingPlayerUnit;
        }
        else if (args.m_OccupyingPlayerUnit == m_OccupyingPlayerUnit)
        {
            m_MeshRenderer.material = m_DefaultMaterial;
            m_OccupyingPlayerUnit = null;
        }
    }
}