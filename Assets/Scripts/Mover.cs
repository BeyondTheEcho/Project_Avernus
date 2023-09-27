using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Mover : MonoBehaviour
{
    private NavMeshAgent m_NavMeshAgent;
    private List<Vector3> m_CurrentPath = new();
    private int m_CurrentWaypointIndex;
    private bool m_IsMoving = false;
    public int m_MovementRange = 4;
    public int m_RemainingMovementRange = 0;
    private int m_CellsToMove = 0;
    private Vector2Int m_Destination;

    private void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        m_CurrentWaypointIndex = 0;
    }

    private void Update()
    {
        if (m_CurrentPath != null && m_CurrentPath.Count > 0)
        {
            Vector3 positionWithoutY = new Vector3(transform.position.x, 0f, transform.position.z);
            Vector3 waypointWithoutY = new Vector3(m_CurrentPath[m_CurrentWaypointIndex].x, 0f, m_CurrentPath[m_CurrentWaypointIndex].z);

            // Check if we've reached the current waypoint
            if (Vector3.Distance(positionWithoutY, waypointWithoutY) < m_NavMeshAgent.stoppingDistance)
            {
                // Move to the next waypoint
                m_CurrentWaypointIndex++;

                // Check if we've reached the end of the path
                if (m_CurrentWaypointIndex >= m_CurrentPath.Count)
                {
                    // Clear the path
                    m_CurrentPath.Clear();

                    // Stop the NavMeshAgent
                    m_NavMeshAgent.isStopped = true;

                    // Handle path completion (e.g., perform another action)
                    HandlePathCompletion();
                }
                else
                {
                    // Set the destination to the next waypoint
                    m_NavMeshAgent.SetDestination(m_CurrentPath[m_CurrentWaypointIndex]);
                }
            }
        }
    }

    public void OrderNavMeshAgentToFollowPath(List<Vector2Int> path)
    {
        if (m_IsMoving) return;
        if (path.Count > m_RemainingMovementRange) return;

        m_CellsToMove = path.Count;

        m_IsMoving = true;

        // Convert path from grid positions to world positions
        m_CurrentPath.Clear();
        foreach (Vector2Int gridPos in path)
        {
            Vector3 worldPos = new Vector3(gridPos.x, 0f, gridPos.y);
            m_CurrentPath.Add(worldPos);
        }

        // Start following the path from the first waypoint
        m_CurrentWaypointIndex = 0;
        m_NavMeshAgent.isStopped = false;

        // Set the destination to the first waypoint
        m_NavMeshAgent.SetDestination(m_CurrentPath[m_CurrentWaypointIndex]);

        m_Destination = path[path.Count - 1];
    }

    private void HandlePathCompletion()
    {
        m_RemainingMovementRange -= m_CellsToMove;

        transform.position = new Vector3(m_Destination.x, 0f, m_Destination.y);
        GridSystem.s_Instance.MarkCellsInRange(GridSystem.s_Instance.FindCellsInRange(m_Destination, m_RemainingMovementRange));

        m_CellsToMove = 0;
        m_IsMoving = false;
    }

}
