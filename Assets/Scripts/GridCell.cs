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

    private MeshRenderer m_MeshRenderer;
    private Vector2Int m_GridPosition;
    private Unit m_OccupyingUnit;


    private void Awake()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InteractionHandler.s_Instance.a_GridCellHovered += SetHovered;
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

    public void SetHovered(InteractionHandler.CellHoveredArgs args)
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
}