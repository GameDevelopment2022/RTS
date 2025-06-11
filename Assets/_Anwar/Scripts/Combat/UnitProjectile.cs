using System;
using Mirror;
using UnityEngine;

public class UnitProjectile : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private int damageToOthers = 20;
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private float lifeTime = 5f;

    private void Start()
    {
        rb.linearVelocity = transform.forward * launchForce;
    }


    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out NetworkIdentity netIdentity))
        {
            if (netIdentity.connectionToClient == connectionToClient)
                return;
        }

        if (other.TryGetComponent(out Health health))
        {
            health.OnDealDamage(damageToOthers);
        }

        DestroyProjectile();
    }

    public override void OnStartServer()
    {
        Invoke(nameof(DestroyProjectile), lifeTime);
    }

    [Server]
    private void DestroyProjectile()
    {
        NetworkServer.Destroy(gameObject);
    }
}