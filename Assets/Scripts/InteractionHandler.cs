using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    public static InteractionHandler s_Instance;

    public Action<UnitSelectedArgs> a_UnitSelected;
    public Action<CellHoveredArgs> a_GridCellHovered;

    [SerializeField] private LayerMask m_GridCellLayerMask;
    [SerializeField] private LayerMask m_UnitLayerMask;

    private GridCell m_LastHoveredGridCell;
    private Unit m_SelectedUnit;

    private const int c_LeftMouseButton = 0;

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
    }

    void Update()
    {
        if(TryHandleUnitSelection()) return;
        HandleUnitMovement();
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, m_UnitLayerMask))
        {
            a_GridCellHovered?.Invoke(new CellHoveredArgs { m_HoveredGridCell = null });
            if (!Input.GetMouseButtonDown(c_LeftMouseButton)) return true;
                
            if (hit.collider.gameObject.TryGetComponent(out Unit unit))
            {
                m_SelectedUnit = unit;

                a_UnitSelected?.Invoke(new UnitSelectedArgs { m_SelectedUnit = unit });

                return true;
            }
        }

        return false;
    }

    private void HandleUnitMovement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, m_GridCellLayerMask))
        {
            if (hit.collider.gameObject.TryGetComponent(out GridCell gridCell))
            {
                a_GridCellHovered?.Invoke(new CellHoveredArgs { m_HoveredGridCell = gridCell });

                if (Input.GetMouseButtonDown(c_LeftMouseButton))
                {
                    if (m_SelectedUnit == null) return;

                    StartCoroutine(OrderUnitToMove(m_SelectedUnit, gridCell));
                }
            }
        }
        else
        {
            a_GridCellHovered?.Invoke(new CellHoveredArgs { m_HoveredGridCell = null });
        }
    }

    private IEnumerator OrderUnitToMove(Unit unit, GridCell destination)
    {
        yield return unit.MoveTo(destination.transform.position);

        destination.SetOccupyingUnit(unit);
    }

    public struct UnitSelectedArgs
    {
        public Unit m_SelectedUnit;
    }

    public struct CellHoveredArgs
    {
        public GridCell m_HoveredGridCell;
    }
}