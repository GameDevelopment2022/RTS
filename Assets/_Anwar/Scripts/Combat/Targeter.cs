using Mirror;
using Obvious.Soap;
using UnityEngine;

namespace AnwarMajid
{
    public class Targeter : NetworkBehaviour
    {
        [SerializeField] private Targetable targetable;
        [SerializeField] private ScriptableEventVector3 MoveCommand;


        #region Server

        public override void OnStartServer()
        {
            base.OnStartServer();
            MoveCommand.OnRaised += MoveCommandIssued;
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            MoveCommand.OnRaised -= MoveCommandIssued;
        }

        [Command]
        public void CmdSetTarget(GameObject target)
        {
            if (!target.TryGetComponent(out Targetable targetable))
                return;

            this.targetable = targetable;
        }


        [Server]
        public void MoveCommandIssued(Vector3 position)
        {
            ClearTarget();
        }

        [Server]
        public void ClearTarget()
        {
            targetable = null;
        }

        #endregion

        #region Client

        #endregion
    }
}