using Mirror;
using UnityEngine;


namespace AnwarMajid
{

    public class Targetable : NetworkBehaviour
    {
        [SerializeField] private Transform aimAtPoint;

        public Transform AimAtPoint => aimAtPoint;
    }
}