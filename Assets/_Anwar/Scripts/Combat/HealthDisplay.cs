using System;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private GameObject healthBarParent;

    [SerializeField] private Image healthBarImg;


    private void Awake()
    {
        health.ClientOnHealthChange += HandleOnHealthChange;
    }

    private void OnDestroy()
    {
        health.ClientOnHealthChange -= HandleOnHealthChange;
    }

    private void OnMouseEnter()
    {
        healthBarParent.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        healthBarParent.gameObject.SetActive(false);
    }

    private void HandleOnHealthChange(int newHealth, int oldHealth)
    {
        healthBarImg.fillAmount = (float)(newHealth / 100f);
    }
}