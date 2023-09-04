using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IInteractable
{
    private float m_Health = 100f;

    public void InteractWithThis(Unit currentUnit)
    {
        throw new System.NotImplementedException();
    }

    public void AttackThisTarget(Unit currentUnit)
    {
        float distance = Vector3.Distance(transform.position, currentUnit.transform.position);
        Debug.Log(distance);

        if (distance <= currentUnit.m_AttackRange)
        {
            m_Health -= currentUnit.m_AttackDamage;
            Debug.Log(m_Health);
        }
    }
}
