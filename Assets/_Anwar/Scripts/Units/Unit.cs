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

    [SerializeField] public UnitEvents OnServerUnitSpawned;
    [SerializeField] public UnitEvents OnServerUnitDeSpawned;

    [SerializeField] public UnitEvents OnAuthorityUnitSpawned;
    [SerializeField] public UnitEvents OnAuthorityUnitDeSpawned;

    public UnitMovement UnitMovement => unitMovement;
    public Targeter Targeter => targeter;


    #region Server

    public override void OnStartServer()
    {
        OnServerUnitSpawned?.RaiseEvent(this);
    }


    public override void OnStopServer()
    {
        OnServerUnitDeSpawned?.RaiseEvent(this);
    }

    #endregion


    #region Client

    public override void OnStartClient()
    {
        if (!isOwned)
            return;


        OnAuthorityUnitSpawned?.RaiseEvent(this);
    }


    public override void OnStopClient()
    {
        if (!isOwned)
            return;


        OnAuthorityUnitDeSpawned?.RaiseEvent(this);
    }


    [Client]
    public void Select()
    {
        if (!isOwned)
            return;
        OnSelected?.Invoke();
    }


    [Client]
    public void DeSelect()
    {
        if (!isOwned)
            return;
        OnDeSelected?.Invoke();
    }

    #endregion
}