using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Assets.Code;
using System;

[RequireComponent(typeof(Health), typeof(GuardPerception))]
public class GuardAgent : MonoBehaviour
{
    [Header("Sight")]
    [SerializeField] Transform sightSensor;
    [SerializeField] float sightDistance = 6.0f;
    [SerializeField] float fieldOfView = 40.0f;
    [SerializeField] bool drawSight;
    [SerializeField] Color sightRadius, fovLines, playerAngle, playAngleSeen;

    [Header("Patrolling")]
    [SerializeField] float patrollingWalkSpeed = 20.0f;
    [SerializeField] int currentWayPoint = 0;
    [field: SerializeField] public Transform[] PatrolPoints { get; set; }
    [field: SerializeField] public float StopingDistance { get; private set; } = 0.5f;
    [SerializeField] bool drawPath;

    [Header("Chasing")]
    [SerializeField] float reloadTime;
    [SerializeField] float minShootAt, maxShootAt;
    [SerializeField] float minShotFor, maxShootFor;
    [SerializeField] Transform wantedLocation;

    [Header("Other")]
    [SerializeField] float alarmSpeed;
    [field: SerializeField] public float AlarmPullDst { get; private set; } = 8.0f;
    [field: SerializeField] public float DespawnTime { get; private set; } = 8.0f;

    // State Machine
    public StateMachine<GuardAgent> StateMachine { get; set; }
    public Patrol Patrol { get; private set; }
    public Chase Chase { get; private set; }
    public CallForBackup CallForBackup { get; private set; }

    private Health _health;
    private bool _isDead;

    public static event System.Action Alerted;
    public static event System.Action Died;

    public Rigidbody RididBody { get; private set; }
    public NavUnit NavAgent { get; private set; }
    public GuardPerception GuardPerception { get; private set; }
    public GuardAgentsDirector GuardAgentsDirector { get; private set; }
    public Gun Gun { get; set; }
    public Transform Player { get; set; }
    public int CurrentWayPoint { get; private set; }
    public float PatrolPointDst { get; set; }
    [field: SerializeField] public float PatrollingWalkSpeed { get; set; } = 6f;
    public float SpotPlayerAt { get; set; }


    public Transform SightSensor
    {
        get
        {
            return sightSensor;
        }

        set
        {
            sightSensor = value;
        }
    }

    //public float DespawnTime
    //{
    //    get
    //    {
    //        return despawnBody;
    //    }

    //    set
    //    {
    //        despawnBody = value;
    //    }
    //}

    //public float AlarmPullDst
    //{
    //    get
    //    {
    //        return alarmPullDst;
    //    }

    //    set
    //    {
    //        alarmPullDst = value;
    //    }
    //}

    //public Transform WantedLocation
    //{
    //    get
    //    {
    //        return wantedLocation;
    //    }

    //    set
    //    {
    //        wantedLocation = value;
    //    }
    //}n

    public void Awake()
    {
        // Get varables.
        RididBody = GetComponent<Rigidbody>();
        NavAgent = GetComponent<NavUnit>();
        GuardPerception = GetComponent<GuardPerception>();
        Gun = GetComponentInChildren<Gun>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        // Create new states for this agent.
        Patrol = new Patrol();
        Chase = new Chase(this);
        CallForBackup = new CallForBackup();

        // Create new state machine.
        StateMachine = new StateMachine<GuardAgent>(this);
        StateMachine.ChanageState(Patrol);
    }

    private void OnEnable()
    {
        if (GuardAgentsDirector != null)
            GuardAgentsDirector.EnterCombatPhase += () => StateMachine.ChanageState(Chase);
        
        GetComponent<Health>().Die += () => StateMachine.ChanageState(Death.Instance);
        GetComponent<Health>().Die += Died;
    }

    private void OnDisable()
    {
        if (GuardAgentsDirector != null)
            GuardAgentsDirector.EnterCombatPhase -= () => StateMachine.ChanageState(Chase);

        GetComponent<Health>().Die -= () => StateMachine.ChanageState(Death.Instance);
        GetComponent<Health>().Die -= Died;
    }

    public void Update()
    {
        // Update state machine.
        StateMachine.Update();
    }

    //private void OnDrawGizmos()
    //{
    //    if (drawSight)
    //    {
    //        Gizmos.color = sightRadius;
    //        Gizmos.DrawWireSphere(this.transform.position, sightDistance);

    //        Gizmos.color = fovLines;
    //        Vector3 fovLine1 = Quaternion.AngleAxis(fieldOfView, transform.up) * transform.forward * sightDistance;
    //        Vector3 fovLine2 = Quaternion.AngleAxis(-fieldOfView, transform.up) * transform.forward * sightDistance;
    //        Gizmos.DrawRay(this.transform.position, fovLine1);
    //        Gizmos.DrawRay(this.transform.position, fovLine2);

    //        if (CanSeePlayer())
    //            Gizmos.color = playAngleSeen;
    //        else
    //            Gizmos.color = playerAngle;

    //        Gizmos.DrawRay(transform.position, (Player.position - transform.position).normalized * sightDistance);

    //        Gizmos.DrawRay(transform.position, transform.forward * sightDistance);
    //    }
    //}

    /// <summary>
    /// Find the nearst alarm button to pull to call of backup.
    /// </summary>
    /// <returns></returns>
    public Transform FindNearestAlarm()
    {
        // Find variable to be used in a for loop.
        GameObject[] alarmObject = GameObject.FindGameObjectsWithTag("Alarm");
        GameObject nearestAlarm = alarmObject[0];
        float nearestDist = Vector3.Distance(transform.position, alarmObject[0].transform.position);

        // Check all objects with the "Alarm" tag.
        for (int i = 1; i < alarmObject.Length; i++)
        {
            // Check the distance between the current posion and the alarm.
            float dist = Vector3.Distance(transform.position, alarmObject[i].transform.position);

            // Set the current alarm as the nearest when distance is smaller.
            if (dist < nearestDist)
                nearestAlarm = alarmObject[i];
        }

        // Return transform.
        return nearestAlarm.transform;
    }

    public float ShootAt
    {
        get
        {
            return UnityEngine.Random.Range(minShootAt, maxShootAt);
        }
    }

    public float ShootFor
    {
        get
        {
            return UnityEngine.Random.Range(minShotFor, maxShootFor);
        }
    }

    public float ReloadTime
    {
        get
        {
            return reloadTime;
        }

        set
        {
            reloadTime = value;
        }
    }
}