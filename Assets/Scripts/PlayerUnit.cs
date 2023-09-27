using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static GridSystem;

public class PlayerUnit : UnitBase
{
    public override void StartTurn()
    {
        base.StartTurn();
        
        Vector2Int startCell = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        GridSystem.s_Instance.MarkCellsInRange(GridSystem.s_Instance.FindCellsInRange(startCell, (int)m_RemainingMovementRange));
    }
}