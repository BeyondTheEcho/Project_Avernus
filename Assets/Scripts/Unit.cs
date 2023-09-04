using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [SerializeField] private GameObject m_SelectedIndicator;

    private NavMeshAgent m_NavMeshAgent;

    private void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        InteractionHandler.s_Instance.a_UnitSelected += SetSelected;
    }

    private void SetSelected(InteractionHandler.UnitSelectedArgs args)
    {
        if (args.m_SelectedUnit == this)
        {
            m_SelectedIndicator.SetActive(true);
        }
        else
        {
            m_SelectedIndicator.SetActive(false);
        }
    }

    public void MoveTo(Vector3 destination)
    {
        m_NavMeshAgent.SetDestination(destination);
    }
}
