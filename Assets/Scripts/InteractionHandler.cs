using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    public static InteractionHandler s_Instance;

    public Action<UnitSelectedArgs> a_UnitSelected;
    public Action<GridCellHoveredArgs> a_GridCellHovered;
    public Action<GridCellOccupiedArgs> a_GridCellOccupied;

    [SerializeField] private LayerMask m_GridCellLayerMask;
    [SerializeField] private LayerMask m_UnitLayerMask;
    [SerializeField] private LayerMask m_EnemyLayerMask;

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
        if(TryHandleAttackOrder()) return;
        HandleUnitMovement();
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, m_UnitLayerMask))
        {
            a_GridCellHovered?.Invoke(new GridCellHoveredArgs { m_HoveredGridCell = null });
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

    private bool TryHandleAttackOrder()
    {
        if (m_SelectedUnit == null) return false;
        if (!Input.GetMouseButtonDown(c_LeftMouseButton)) return true;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, m_EnemyLayerMask))
        {
            if (hit.collider.gameObject.TryGetComponent(out IInteractable enemy))
            {
                enemy.AttackThisTarget(m_SelectedUnit);

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
                a_GridCellHovered?.Invoke(new GridCellHoveredArgs { m_HoveredGridCell = gridCell });

                if (Input.GetMouseButtonDown(c_LeftMouseButton))
                {
                    if (m_SelectedUnit == null) return;

                    StartCoroutine(OrderUnitToMove(m_SelectedUnit, gridCell));
                }
            }
        }
        else
        {
            a_GridCellHovered?.Invoke(new GridCellHoveredArgs { m_HoveredGridCell = null });
        }
    }

    private IEnumerator OrderUnitToMove(Unit unit, GridCell destination)
    {
        yield return unit.MoveTo(destination.transform.position);

        a_GridCellOccupied?.Invoke(new GridCellOccupiedArgs { m_OccupiedGridCell = destination, m_OccupyingUnit = unit });
    }

    public struct UnitSelectedArgs
    {
        public Unit m_SelectedUnit;
    }

    public struct GridCellHoveredArgs
    {
        public GridCell m_HoveredGridCell;
    }

    public struct GridCellOccupiedArgs
    {
        public GridCell m_OccupiedGridCell;
        public Unit m_OccupyingUnit;
    }
}