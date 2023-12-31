using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitTurnGameState : IGameState
{
    private readonly UnitBase m_CurrentUnit;

    public PlayerUnitTurnGameState(UnitBase currentPlayerUnit)
    {
        m_CurrentUnit = currentPlayerUnit;
    }

    public void OnEnter()
    {
        ((PlayerUnit)m_CurrentUnit).StartTurn();
    }

    public void OnExit()
    {
        m_CurrentUnit.EndTurn();
    }

    public void OnUpdate()
    {
        HandleUnitMovement();
    }

    private void HandleUnitMovement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, GridSystem.s_Instance.m_LayerMasks.m_GridCellLayerMask))
        {
            if (hit.collider.gameObject.TryGetComponent(out GridCell gridCell))
            {
                GridSystem.s_Instance.a_GridCellHovered?.Invoke(new GridSystem.GridCellHoveredArgs { m_HoveredGridCell = gridCell });

                Vector2Int startCell = new Vector2Int((int)m_CurrentUnit.transform.position.x, (int)m_CurrentUnit.transform.position.z);
                var path = GridSystem.s_Instance.FindPath(startCell, gridCell.GetGridPosition());
                GridSystem.s_Instance.HighlightPath(path);

                if (Input.GetMouseButtonDown(MouseButtons.c_LeftMouseButton))
                {
                    m_CurrentUnit.MoveTo(path);
                }
            }
        }
        else
        {
            GridSystem.s_Instance.a_GridCellHovered?.Invoke(new GridSystem.GridCellHoveredArgs { m_HoveredGridCell = null });
        }
    }
}