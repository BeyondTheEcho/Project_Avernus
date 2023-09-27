using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static GridSystem;

[RequireComponent(typeof(Mover))]
public class UnitBase : MonoBehaviour
{
    [SerializeField] private GameObject m_SelectedIndicator;

    protected float m_Health = 100f;
    protected float m_AttackDamage = 5f;
    protected float m_AttackRange = 1f;

    protected Mover m_Mover;

    private void Awake()
    {
        m_Mover = GetComponent<Mover>();
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

    public void MoveTo(List<Vector2Int> path)
    {
        m_Mover.OrderNavMeshAgentToFollowPath(path);
    }

    public void TakeDamage(float damage)
    {
        m_Health -= damage;

        if (m_Health <= 0f)
        {
            Destroy(gameObject);
        }
    }


    public virtual void StartTurn()
    {
        m_Mover.m_RemainingMovementRange = m_Mover.m_MovementRange;
        CameraController.s_Instance.MoveCameraFocusTo(transform.position);
        SetSelected();
    }

    public virtual void EndTurn()
    {
        SetDeselected();
    }
}