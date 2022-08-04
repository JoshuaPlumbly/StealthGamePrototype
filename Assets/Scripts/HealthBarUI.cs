using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class HealthBarUI : MonoBehaviour
{
    [SerializeField] Image healthBar;

    private Health _health;

    public void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _health.Damaged += RefreshHealthBar;
    }

    private void OnDisable()
    {
        _health.Damaged -= RefreshHealthBar;
    }

    public void RefreshHealthBar()
    {
        // Update health bar.
        healthBar.fillAmount = _health.GetHitpointsFill();
    }
}