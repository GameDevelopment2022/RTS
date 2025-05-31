using System;
using Mirror;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.AI;

namespace AnwarMajid
{
    public class UnitMovement : NetworkBehaviour
    {
        [SerializeField] private NavMeshAgent agent = null;
        [SerializeField] private ScriptableEventVector3 MoveCommand;

        #region Server

        [ServerCallback]
        private void Update()
        {
            if (!agent.hasPath)
            {
                return;
            }


            if (agent.remainingDistance > agent.stoppingDistance)
            {
                return;
            }

            agent.ResetPath();
        }

        [Command]
        public void CmdMove(Vector3 position)
        {
            Debug.Log($"[Command] Received move command to position: {position}");

            if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                Debug.LogWarning("[Command] Invalid move position. Could not sample NavMesh.");
                return;
            }

            Debug.Log($"[Command] Valid position found at: {hit.position}. Setting destination.");
            agent.SetDestination(position);

            if (isOwned)
                MoveCommand?.Raise(position);
        }

        #endregion
    }
}