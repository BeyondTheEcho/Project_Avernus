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
    private Unit m_OccupyingUnit;

    // Start is called before the first frame update
    void Start()
    {
        InteractionHandler.s_Instance.a_GridCellHovered += OnHovered;
        InteractionHandler.s_Instance.a_GridCellOccupied += OnOccupied;
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

    public void SetOccupyingUnit(Unit unit)
    {
        m_OccupyingUnit = unit;
        m_MeshRenderer.material = m_OccupiedMaterial;
    }

    public void ClearOccupyingUnit()
    {
        m_OccupyingUnit = null;
        m_MeshRenderer.material = m_DefaultMaterial;
    }

    public void OnHovered(InteractionHandler.GridCellHoveredArgs args)
    {
        if (m_OccupyingUnit != null) return;

        if (args.m_HoveredGridCell == this)
        {
            m_MeshRenderer.material = m_HoveredMaterial;
        }
        else
        {
            m_MeshRenderer.material = m_DefaultMaterial;
        }
    }

    public void OnOccupied(InteractionHandler.GridCellOccupiedArgs args)
    {
        if (args.m_OccupiedGridCell == this)
        {
            m_MeshRenderer.material = m_OccupiedMaterial;
            m_OccupyingUnit = args.m_OccupyingUnit;
        }
        else if (args.m_OccupyingUnit == m_OccupyingUnit)
        {
            m_MeshRenderer.material = m_DefaultMaterial;
            m_OccupyingUnit = null;
        }
    }
}