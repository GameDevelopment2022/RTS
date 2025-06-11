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
        [SerializeField] private Targeter targeter = null;
        [SerializeField] private float chaseRange = 10f;

        #region Server

        [ServerCallback]
        private void Update()
        {
            Targetable target = targeter.Targetable;


            if (target != null)
            {
                if ((target.transform.position - transform.position).sqrMagnitude > chaseRange * chaseRange)
                {
                    agent.SetDestination(target.transform.position);
                    return;
                }
                else if (agent.hasPath)
                {
                    agent.ResetPath();
                    return;
                }

                return;
            }


            if (!agent.hasPath)
                return;

            if (agent.remainingDistance > agent.stoppingDistance)
                return;

            agent.ResetPath();
        }

         [Command]
        public void CmdMove(Vector3 position)
        {
            if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                return;
            }

            agent.SetDestination(position);
            if (isOwned)
                targeter.ResetTarget();
        }

        #endregion
    }
}