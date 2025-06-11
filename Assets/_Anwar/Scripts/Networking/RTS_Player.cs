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



    #region Server

    public override void OnStartServer()
    {
        Unit.OnServerUnitSpawned += ServerUnitSpawned;
        Unit.OnServerUnitDeSpawned += ServerUnitDeSpawned;
    }

    public override void OnStopServer()
    {
        Unit.OnServerUnitSpawned -= ServerUnitSpawned;
        Unit.OnServerUnitDeSpawned -= ServerUnitDeSpawned;
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

        Unit.OnAuthorityUnitSpawned += AuthorityUnitSpawned;
        Unit.OnAuthorityUnitDeSpawned += AuthorityUnitDeSpawned;
    }

    public override void OnStopClient()
    {
        if (!isClientOnly)
            return;

        Unit.OnAuthorityUnitSpawned -= AuthorityUnitSpawned;
        Unit.OnAuthorityUnitDeSpawned -= AuthorityUnitDeSpawned;
    }


    private void AuthorityUnitSpawned(Unit unit)
    {
        if (!authority)
            return;
        myUnits.Add(unit);
    }

    private void AuthorityUnitDeSpawned(Unit unit)
    {
        if (!authority)
            return;
        myUnits.Remove(unit);
    }

    #endregion
}