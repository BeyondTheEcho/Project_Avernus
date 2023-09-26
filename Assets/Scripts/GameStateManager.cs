using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private IGameState m_CurrentGameState;

    private List<UnitBase> m_AllUnits = new();

    void Awake()
    {
        m_AllUnits.AddRange(FindObjectsOfType<UnitBase>());
    }

    void Start()
    {
        CreateNewUnitState(m_AllUnits.First());
    }

    void Update()
    {
        m_CurrentGameState.OnUpdate();
    }

    public void EndTurn()
    {
        m_AllUnits.PopAppend();

        CreateNewUnitState(m_AllUnits.First());
    }

    private void CreateNewUnitState(UnitBase unit)
    {
        if (unit is PlayerUnit)
        {
            PushState(new PlayerUnitTurnGameState(unit));
        }
        else if (unit is EnemyUnit)
        {
            PushState(new EnemyUnitTurnGameState(unit));
        }
    }

    private void PushState(IGameState state)
    {
        m_CurrentGameState?.OnExit();

        m_CurrentGameState = state;

        m_CurrentGameState.OnEnter();
    }
}