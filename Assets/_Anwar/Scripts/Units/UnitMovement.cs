using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace AnwarMajid
{

    public class UnitMovement : NetworkBehaviour
    {
        [SerializeField] private NavMeshAgent agent = null;

        #region Server

        [Command]
        public void CmdMove(Vector3 position)
        {
            if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) return;

            agent.SetDestination(position);
        }

        #endregion

    }
}
