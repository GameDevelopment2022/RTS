using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RTS_Player : NetworkBehaviour
{
    [SerializeField] private List<Unit> myUnits = new List<Unit>();

    public List<Unit> MyUnits
    {
        get { return myUnits; }
    }

    [SerializeField] private UnitEvents OnServerUnitSpawned;
    [SerializeField] private UnitEvents OnServerUnitDeSpawned;

    [SerializeField] private UnitEvents OnAuthorityUnitSpawned;
    [SerializeField] private UnitEvents OnAuthorityUnitDeSpawned;


    #region Server

    public override void OnStartServer()
    {
        OnServerUnitSpawned.OnEventRaised += ServerUnitSpawned;
        OnServerUnitDeSpawned.OnEventRaised += ServerUnitDeSpawned;
    }

    public override void OnStopServer()
    {
        OnServerUnitSpawned.OnEventRaised -= ServerUnitSpawned;
        OnServerUnitDeSpawned.OnEventRaised -= ServerUnitDeSpawned;
    }


    private void ServerUnitSpawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId)
            return;
        myUnits.Add(unit);
    }

    private void ServerUnitDeSpawned(Unit unit)
    {
        if (unit.connectionToClient.connectionId != connectionToClient.connectionId)
            return;
        myUnits.Remove(unit);
    }

    #endregion


    #region Client

    public override void OnStartClient()
    {
        if (!isClientOnly)
            return;

        OnAuthorityUnitSpawned.OnEventRaised += AuthorityUnitSpawned;
        OnAuthorityUnitDeSpawned.OnEventRaised += AuthorityUnitDeSpawned;
    }

    public override void OnStopClient()
    {
        if (!isClientOnly)
            return;

        OnAuthorityUnitSpawned.OnEventRaised -= AuthorityUnitSpawned;
        OnAuthorityUnitDeSpawned.OnEventRaised -= AuthorityUnitDeSpawned;
    }


    private void AuthorityUnitSpawned(Unit unit)
    {
        if (!isOwned)
            return;
        myUnits.Add(unit);
    }

    private void AuthorityUnitDeSpawned(Unit unit)
    {
        if (!isOwned)
            return;
        myUnits.Remove(unit);
    }

    #endregion
}