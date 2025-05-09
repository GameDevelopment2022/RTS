using Mirror;
using UnityEngine;

namespace AnwarMajid
{
    public class RTS_NetworkManager : NetworkManager
    {

        [SerializeField] private GameObject unitSpawnerPrefab = null;


        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);

            var spawnerInstace = Instantiate(unitSpawnerPrefab, conn.identity.transform.position, conn.identity.transform.rotation);

            NetworkServer.Spawn(spawnerInstace, conn);
        }
    }
}
