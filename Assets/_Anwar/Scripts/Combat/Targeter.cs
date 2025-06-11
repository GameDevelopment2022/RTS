using Mirror;
using Obvious.Soap;
using UnityEngine;

namespace AnwarMajid
{
    public class Targeter : NetworkBehaviour
    {
        [SerializeField] private Targetable targetable;
        public Targetable Targetable => targetable;

        #region Server

        [Command]
        public void CmdSetTarget(GameObject target)
        {
            if (!target.TryGetComponent(out Targetable targetComp))
                return;

            this.targetable = targetComp;
        }


        public void ResetTarget()
        {
            targetable = null;
        }

        #endregion
    }
}