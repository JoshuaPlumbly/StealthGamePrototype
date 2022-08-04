using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Other/BullectStats")]
public class BulletStats : ScriptableObject
{
    public float damage;
    public float startSpeed;
    public float dropRate;
    public float drag;
}