using System;
using Mirror;
using UnityEngine;

public class UnitProjectile : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private float lifeTime = 5f;

    private void Start()
    {
        rb.linearVelocity = transform.forward * launchForce;
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