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

    protected float m_Health = 100f;
    protected float m_AttackDamage = 5f;
    protected float m_AttackRange = 1f;
    protected float m_MovementRange = 4;
    protected float m_RemainingMovementRange = 0;

    private bool m_IsMoving = false;

    private NavMeshAgent m_NavMeshAgent;

    private void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        GridSystem.s_Instance.GetCellAtPos(transform.position).SetOccupyingUnit(this);
    }

    public void SetSelected()
    {
        m_SelectedIndicator.SetActive(true);
    }

    public void SetDeselected()
    {
        m_SelectedIndicator.SetActive(false);
    }

    public bool IsGridCellInRange(GridCell cell)
    {
        float destinationDistance = GridSystem.Get2DDistanceAsFloat(transform.position, cell.transform.position);

        return destinationDistance <= m_RemainingMovementRange;
    }

    public void MoveTo(GridCell destination)
    {
        if (m_IsMoving) return;
        if (!IsGridCellInRange(destination)) return;

        float destinationDistance = GridSystem.Get2DDistanceAsFloat(transform.position, destination.transform.position);

        StartCoroutine(MoveToCoroutine(destination.transform.position, () =>
        {
            GridSystem.s_Instance.a_GridCellOccupied?.Invoke(new GridCellOccupiedArgs { m_OccupiedGridCell = destination, m_OccupyingPlayerUnit = this as PlayerUnit });
            m_IsMoving = false;
            m_RemainingMovementRange -= (int)destinationDistance;

            Vector2Int startCell = new Vector2Int((int)transform.position.x, (int)transform.position.z);
            GridSystem.s_Instance.MarkCellsInRange(GridSystem.s_Instance.FindCellsInRange(startCell, (int)m_RemainingMovementRange));
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

    public float GetMovementRange()
    {
        return m_MovementRange;
    }

    public float GetRemainingMovementRange()
    {
        return m_RemainingMovementRange;
    }

    public virtual void StartTurn()
    {
        m_RemainingMovementRange = m_MovementRange;
        CameraController.s_Instance.MoveCameraFocusTo(transform.position);
        SetSelected();
    }

    public virtual void EndTurn()
    {
        SetDeselected();
    }
}