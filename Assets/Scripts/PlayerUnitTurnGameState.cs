using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InteractionHandler;

public class PlayerUnitTurnGameState : IGameState
{
    private readonly UnitBase m_CurrentUnit;

    public PlayerUnitTurnGameState(UnitBase currentPlayerUnit)
    {
        m_CurrentUnit = currentPlayerUnit;

        OnEnter();
    }

    public void OnEnter()
    {
        m_CurrentUnit.StartTurn();
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

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, InteractionHandler.s_Instance.m_LayerMasks.m_GridCellLayerMask))
        {
            if (hit.collider.gameObject.TryGetComponent(out GridCell gridCell))
            {
                InteractionHandler.s_Instance.a_GridCellHovered?.Invoke(new GridCellHoveredArgs { m_HoveredGridCell = gridCell });

                if (Input.GetMouseButtonDown(MouseButtons.c_LeftMouseButton))
                {
                    InteractionHandler.s_Instance.MoveUnit((PlayerUnit)m_CurrentUnit, gridCell);
                }
            }
        }
        else
        {
            InteractionHandler.s_Instance.a_GridCellHovered?.Invoke(new GridCellHoveredArgs { m_HoveredGridCell = null });
        }
    }
}