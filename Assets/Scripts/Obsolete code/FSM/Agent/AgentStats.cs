using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentStats : MonoBehaviour
{
    [Header("Movement")]
    public int currentWayPoint;
    public Transform[] wayPointList;
    public float stopingDistance;

    [Header("Sight")]
    public Transform eyes;
    public float sight;
    public float identifyPlayerAt;

    [Header("Combat")]
    public Transform target;
    public Gun selectedWeapon;
    public float minShootDist;
    public float maxShootDist;
    public float knifeCharage;

    private void Awake()
    {
        selectedWeapon = GetComponentInChildren<Gun>();
    }
}
