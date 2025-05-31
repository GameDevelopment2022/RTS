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

    [SerializeReference] private UnitMovement unitMovement;
    [SerializeReference] private Targeter targeter;

    [SerializeReference] public UnitEvents OnServerUnitSpawned;
    [SerializeReference] public UnitEvents OnServerUnitDeSpawned;

    [SerializeReference] public UnitEvents OnAuthorityUnitSpawned;
    [SerializeReference] public UnitEvents OnAuthorityUnitDeSpawned;

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
        if (!isClientOnly || !isOwned)
            return;


        OnAuthorityUnitSpawned?.RaiseEvent(this);
    }


    public override void OnStopClient()
    {
        if (!isClientOnly || !isOwned)
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