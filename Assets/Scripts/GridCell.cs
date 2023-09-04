using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField] Material m_DefaultMaterial;
    [SerializeField] Material m_HoveredMaterial;

    private MeshRenderer m_MeshRenderer;
    private Vector2Int m_GridPosition;
    [SerializeField] private TMP_Text m_GridCellText;

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

    public void SetHovered(InteractionHandler.CellHoveredArgs args)
    {
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