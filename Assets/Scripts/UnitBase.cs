using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static GridSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitBase : MonoBehaviour
{
    [SerializeField] private GameObject m_SelectedIndicator;

    private float m_Health = 100f;
    private float m_AttackDamage = 5f;
    private float m_AttackRange = 1f;

    private bool m_IsMoving = false;

    private NavMeshAgent m_NavMeshAgent;

    private void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void SetSelected()
    {
        m_SelectedIndicator.SetActive(true);
    }

    public void SetDeselected()
    {
        m_SelectedIndicator.SetActive(false);
    }

    public void MoveTo(GridCell destination)
    {
        if (m_IsMoving) return;

        StartCoroutine(MoveToCoroutine(destination.transform.position, () =>
        {
            GridSystem.s_Instance.a_GridCellOccupied?.Invoke(new GridCellOccupiedArgs { m_OccupiedGridCell = destination, m_OccupyingPlayerUnit = this as PlayerUnit });
            m_IsMoving = false;
        }));
    }

    private IEnumerator MoveToCoroutine(Vector3 destination, Action onComplete = null)
    {
        m_IsMoving = true;

        if (m_NavMeshAgent.hasPath) yield break;

        m_NavMeshAgent.SetDestination(destination);

        yield return new WaitUntil(() => Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(destination.x, destination.z)) <= 0.1f);

        transform.position = destination;

        onComplete?.Invoke();

        m_IsMoving = false;
    }

    public void TakeDamage(float damage)
    {
        m_Health -= damage;

        if (m_Health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void StartTurn()
    {
        SetSelected();
    }

    public void EndTurn()
    {
        SetDeselected();
    }
}