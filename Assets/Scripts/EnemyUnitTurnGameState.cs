using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitTurnGameState : IGameState
{
    private readonly UnitBase m_CurrentUnit;

    public EnemyUnitTurnGameState(UnitBase currentPlayerUnit)
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
        //Enemy unit AI logic goes here
    }
}