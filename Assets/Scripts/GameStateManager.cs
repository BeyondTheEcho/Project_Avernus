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
        m_CurrentGameState = new UnitTurnGameState(m_AllUnits[0]);
    }

    void Update()
    {
        m_CurrentGameState.OnEnter();
        m_CurrentGameState.OnUpdate();
    }
}
