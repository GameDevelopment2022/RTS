using UnityEngine;
using Mirror;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


namespace AnwarMajid
{
    public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject unitPrefab = null;
        [SerializeField] private Transform spawnPosition = null;



        #region Server

        [Command]
        private void CmdSpawnUnit()
        {
            var unitInstance = Instantiate(unitPrefab, spawnPosition.position, spawnPosition.rotation);
            NetworkServer.Spawn(unitInstance, connectionToClient);
        }

        #endregion


        #region Client
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            if (!authority)
                return;
            CmdSpawnUnit();
        }

        #endregion

    }
}
