using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    abstract void InteractWithThis(PlayerUnit currentPlayerUnit);
    abstract void AttackThisTarget(PlayerUnit currentPlayerUnit);
}
