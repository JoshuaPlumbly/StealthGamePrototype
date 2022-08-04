using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, ITakeDamage
{
    [field: SerializeField] public int HitPoints { get; private set; } = 100;
    [field: SerializeField] public int MaxHitPoints { get; private set; } = 100;

    public event System.Action Die;
    public event System.Action Damaged;

    public void TakeDamage(int damage)
    {
        // Check if damage is not negateive
        if (damage < 0)
            return;
        
        HitPoints -= damage;
        Damaged?.Invoke();

        // Prevent health from being negative.
        if (HitPoints < 0)
            HitPoints = 0;

        // Check if health is zero
        if (HitPoints <= 0)
            Die?.Invoke();
    }

    public float GetHitpointsFill() => (float)HitPoints / MaxHitPoints;
}