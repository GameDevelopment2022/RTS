using UnityEngine;
using Mirror;
using UnityEngine.Events;
using AnwarMajid;
public class Unit : NetworkBehaviour
{
    [SerializeField] private UnityEvent OnSelected;
    [SerializeField] private UnityEvent OnDeSelected;

    [SerializeReference] private UnitMovement unitMovement;

    [SerializeReference] public UnitEvents OnServerUnitSpawned;
    [SerializeReference] public UnitEvents OnServerUnitDeSpawned;

    [SerializeReference] public UnitEvents OnAuthorityUnitSpawned;
    [SerializeReference] public UnitEvents OnAuthorityUnitDeSpawned;

    public UnitMovement GetUnitMovement()
    {
        return unitMovement;
    }

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
        if (!isClientOnly || !authority)
            return;


        OnAuthorityUnitSpawned?.RaiseEvent(this);
    }


    public override void OnStopClient()
    {
        if (!isClientOnly || !authority)
            return;


        OnAuthorityUnitDeSpawned?.RaiseEvent(this);
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
