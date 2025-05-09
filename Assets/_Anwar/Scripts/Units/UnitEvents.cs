using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RTS/Units", fileName = "UnitEvent")]
public class UnitEvents : ScriptableObject
{
    public event Action<Unit> OnEventRaised;

    public void RaiseEvent(Unit unit)
    {
        OnEventRaised?.Invoke(unit);
    }
}