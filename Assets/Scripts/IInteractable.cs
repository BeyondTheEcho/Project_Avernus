using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    abstract void InteractWithThis(Unit currentUnit);
    abstract void AttackThisTarget(Unit currentUnit);
}
