using System;
using Mirror;
using UnityEngine;

namespace AnwarMajid
{
    public class UnitFiring : NetworkBehaviour
    {
        [SerializeField] private Targeter targeter = null;
        [SerializeField] private GameObject projectilePrefab = null;
        [SerializeField] private Transform projectileSpawnPositon = null;

        [SerializeField] private float fireRange = 5f;
        [SerializeField] private float fireRate = 1f;
        [SerializeField] private float rotationSpeed = 20f;


        private float lastFireTime = 0f;

        private Targetable target;

        [ServerCallback]
        private void Update()
        {
            target = targeter.Targetable;

            if (target == null)
                return;

            if (!CanFireAtTarget())
                return;

            Quaternion targetRotation =
                Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


            if (Time.time > (1 / fireRate) + lastFireTime)
            {
                Quaternion projectileRotation =
                    Quaternion.LookRotation(target.AimAtPoint.position - projectileSpawnPositon.position);

                GameObject projectileInstance = Instantiate(projectilePrefab, projectileSpawnPositon.position,
                    projectileRotation);


                NetworkServer.Spawn(projectileInstance, connectionToClient);

                lastFireTime = Time.time;
            }
        }

        [Server]
        private bool CanFireAtTarget()
        {
            return (target.transform.position - transform.position).sqrMagnitude <= fireRange * fireRange;
        }
    }
}