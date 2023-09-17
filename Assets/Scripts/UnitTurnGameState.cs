using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTurnGameState : IGameState
{
    private readonly UnitBase m_CurrentUnit;

    public UnitTurnGameState(UnitBase currentPlayerUnit)
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
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        m_CurrentUnit.EndTurn();
    }
}
