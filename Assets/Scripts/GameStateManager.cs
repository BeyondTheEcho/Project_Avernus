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
        m_CurrentGameState.OnExit();

        m_AllUnits.PopAppend();

        CreateNewUnitState(m_AllUnits.First());
    }

    private void CreateNewUnitState(UnitBase unit)
    {
        if (unit is PlayerUnit)
        {
            m_CurrentGameState = new PlayerUnitTurnGameState(unit);
        }
        else if (unit is EnemyUnit)
        {
            m_CurrentGameState = new EnemyUnitTurnGameState(unit);
        }
    }
}