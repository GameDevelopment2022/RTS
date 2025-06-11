using System;
using Mirror;
using Obvious.Soap;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;


    [SyncVar(hook = nameof(HandleHealthUpdated))] [SerializeField]
    private int currentHealth;

    [SerializeField] public event Action ServerOnDie;
    [SerializeField] public event Action<int, int> ClientOnHealthChange;

    #region Server

    public override void OnStartServer()
    {
        currentHealth = maxHealth;
    }

    [Server]
    public void OnDealDamage(int damageAmount)
    {
        if (currentHealth <= 0)
            return;

        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);

        if (currentHealth != 0)
            return;

        ServerOnDie?.Invoke();
        // Dead

        Debug.Log("Died");
    }

    #endregion

    #region Client

    private void HandleHealthUpdated(int oldHealth, int newHealth)
    {
        ClientOnHealthChange?.Invoke(newHealth, maxHealth);
    }

    #endregion
}