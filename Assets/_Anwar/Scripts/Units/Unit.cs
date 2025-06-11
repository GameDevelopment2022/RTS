using System;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using AnwarMajid;

public class Unit : NetworkBehaviour
{
    [SerializeField] private UnitType unitType;

    public UnitType UnitType => unitType;

    [SerializeField] private UnityEvent OnSelected;
    [SerializeField] private UnityEvent OnDeSelected;

    [SerializeField] private UnitMovement unitMovement;
    [SerializeField] private Targeter targeter;

    [SerializeField] public static event Action<Unit> OnServerUnitSpawned;
    [SerializeField] public static event Action<Unit> OnServerUnitDeSpawned;

    [SerializeField] public static event Action<Unit> OnAuthorityUnitSpawned;
    [SerializeField] public static event Action<Unit> OnAuthorityUnitDeSpawned;

    public UnitMovement UnitMovement => unitMovement;
    public Targeter Targeter => targeter;


    #region Server

    public override void OnStartServer()
    {
        OnServerUnitSpawned?.Invoke(this);
    }


    public override void OnStopServer()
    {
        OnServerUnitDeSpawned?.Invoke(this);
    }

    #endregion


    #region Client

    public override void OnStartClient()
    {
        if (!authority)
            return;


        OnAuthorityUnitSpawned?.Invoke(this);
    }


    public override void OnStopClient()
    {
        if (!authority)
            return;


        OnAuthorityUnitDeSpawned?.Invoke(this);
    }


    [Client]
    public void Select()
    {
        if (!authority)
            return;
        OnSelected?.Invoke();
    }


    [Client]
    public void DeSelect()
    {
        if (!authority)
            return;
        OnDeSelected?.Invoke();
    }

    #endregion
}