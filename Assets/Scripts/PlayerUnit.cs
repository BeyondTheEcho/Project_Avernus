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
        GridSystem.s_Instance.a_MoveAction?.Invoke(new MoveActionArgs { m_Unit = this, m_HighlightOnlyValidMoves = true});
    }
}