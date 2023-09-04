using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [SerializeField] private GameObject m_SelectedIndicator;

    private float m_Health = 100f;
    public float m_AttackDamage = 5f;
    public float m_AttackRange = 1f;

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

    public Coroutine MoveTo(Vector3 destination)
    {
        return StartCoroutine(MoveToCoroutine(destination));
    }

    private IEnumerator MoveToCoroutine(Vector3 destination)
    {
        if (m_NavMeshAgent.hasPath) yield break;

        m_NavMeshAgent.SetDestination(destination);

        yield return new WaitUntil(() => Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(destination.x, destination.z)) <= 0.1f);

        transform.position = destination;
    }
}