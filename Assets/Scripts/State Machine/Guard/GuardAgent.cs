using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Assets.Code;
using System;

[RequireComponent(typeof(Health))]
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
    [SerializeField] float stopingDistance = 0.5f;
    [SerializeField] bool drawPath;
    [Header("Chasing")]
    [SerializeField] float reloadTime;
    [SerializeField] float minShootAt, maxShootAt;
    [SerializeField] float minShotFor, maxShootFor;
    [SerializeField] Transform wantedLocation;

    [Header("Other")]
    [SerializeField] float alarmSpeed;
    [SerializeField] float alarmPullDst;
    [SerializeField] float despawnBody = 8.0f;

    // State Machine
    public StateMachine<GuardAgent> StateMachine { get; set; }
    public Patrol Patrol { get; private set; }
    public Chase Chase { get; private set; }
    public CallForBackup CallForBackup { get; private set; }

    private Health _health;
    private bool _isDead;

    public static event System.Action Alerted;
    public static event System.Action Died;

    #region Encapsulated values
    public Rigidbody RididBody { get; private set; }
    public NavUnit NavAgent { get; private set; }
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

    public float StopingDistance
    {
        get
        {
            return stopingDistance;
        }

        set
        {
            stopingDistance = value;
        }
    }

    public float DespawnTime
    {
        get
        {
            return despawnBody;
        }

        set
        {
            despawnBody = value;
        }
    }

    public float AlarmPullDst
    {
        get
        {
            return alarmPullDst;
        }

        set
        {
            alarmPullDst = value;
        }
    }

    public Transform WantedLocation
    {
        get
        {
            return wantedLocation;
        }

        set
        {
            wantedLocation = value;
        }
    }
    #endregion

    public void Awake()
    {
        // Get varables.
        RididBody = GetComponent<Rigidbody>();
        NavAgent = GetComponent<NavUnit>();
        Gun = GetComponentInChildren<Gun>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        // Create new states for this agent.
        Patrol = new Patrol();
        Chase = new Chase();
        CallForBackup = new CallForBackup();

        // Create new state machine.
        StateMachine = new StateMachine<GuardAgent>(this);
        StateMachine.ChanageState(Patrol);
    }

    private void OnEnable()
    {
        GuardAgentsDirector.BackupRequested += () => StateMachine.ChanageState(Chase);
        GetComponent<Health>().Die += () => StateMachine.ChanageState(Death.Instance);
        GetComponent<Health>().Die += Died;
    }

    private void OnDisable()
    {
        GuardAgentsDirector.BackupRequested -= () => StateMachine.ChanageState(Chase);
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

    #region 
    public bool CanSeePlayer() => InSightCheck(sightSensor, Player, fieldOfView, sightDistance);

    public static bool InSightCheck(Transform checkingObject, Transform target, float maxAngle, float _radis)
    {
        // Check objects that are with ranage.
        Collider[] overlaps = new Collider[30];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, _radis, overlaps);

        // Null check
        if (overlaps == null)
            return false;

        // For each collider within of touching overlap sphere.
        for (int i = 0; i < count + 1; i++)
        {
            if (overlaps[i] != null)
            {
                // Check to see if overlap i is a target.
                if (overlaps[i].transform == target)
                {
                    // Find the direction between the seeker and the seeked.
                    Vector3 directionBetween = (target.position - checkingObject.position).normalized;
                    directionBetween.y *= 0;
                    float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                    // Check object is with field of view.
                    if (angle <= maxAngle)
                    {
                        // Create a raycast to check that there is nothing braking the seekers line of sight.
                        Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, _radis))
                        {
                            // Target is insight.
                            if (hit.transform == target)
                                return true;
                        }
                    }
                }
            }
        }

        // Target is not insight.
        return false;
    }

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
    #endregion
}