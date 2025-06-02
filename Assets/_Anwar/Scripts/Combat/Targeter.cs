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

        // There is a bug that command is not being used
        // [Command] 
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