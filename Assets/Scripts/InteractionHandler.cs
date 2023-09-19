using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    public static InteractionHandler s_Instance;

    [SerializeField] public LayerMasks m_LayerMasks;

    public Action<GridCellHoveredArgs> a_GridCellHovered;
    public Action<GridCellOccupiedArgs> a_GridCellOccupied;

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

    public void MoveUnit(PlayerUnit playerUnit, GridCell destination)
    {
        StartCoroutine(OrderUnitToMove(playerUnit, destination));
    }

    private IEnumerator OrderUnitToMove(PlayerUnit playerUnit, GridCell destination)
    {
        yield return playerUnit.MoveTo(destination.transform.position);

        a_GridCellOccupied?.Invoke(new GridCellOccupiedArgs { m_OccupiedGridCell = destination, OccupyingPlayerUnit = playerUnit });
    }

    public struct GridCellHoveredArgs
    {
        public GridCell m_HoveredGridCell;
    }

    public struct GridCellOccupiedArgs
    {
        public GridCell m_OccupiedGridCell;
        public PlayerUnit OccupyingPlayerUnit;
    }
}