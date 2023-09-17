using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private Queue<PlayerUnit> m_TurnQueue = new Queue<PlayerUnit>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum TurnState
    {
        Move,
        Attack,
        Interact,
        EndTurn
    }
}
