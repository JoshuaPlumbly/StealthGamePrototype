using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Unity3D. “Stealth Game Tutorial - 403 - Enemy Sight and Hearing - Unity Official Tutorials.” YouTube, YouTube, 3 July 2013, www.youtube.com/watch?v=mBGUY7EUxXQ.
/// </summary>

public class NPCMove : MonoBehaviour
{
    enum Status
    {
        idel = 0,
        patroling = 1
    };

    NavMeshAgent _NMA;

    [SerializeField] Status currentStatus;

    [SerializeField] GameObject player;

    [Header("Enemy Sight")]
    [SerializeField] float sightDistance = 10.0f;
    [SerializeField] float fildOfViewAngle = 110.0f;
    [SerializeField] bool playerInSight;
    [SerializeField] Vector3 personalLasSighting;

    [Header("Partol settings")]
    [SerializeField] int partrolID;
    [SerializeField] float minDistFromPartolPoint = 1.0f;
    [SerializeField] bool isWaiting;
    [SerializeField] GameObject[] partolPoints;
    [SerializeField] float[] waitTimes;
    [SerializeField] internal int currPartolPt = 0;

    float stopWaitingAt;
    public Transform destination;

    private void Awake()
    {
        _NMA = this.GetComponent<NavMeshAgent>();
        
        if (_NMA == null)
            Debug.LogWarning(this.name + " dose not have a NavMeshAgent assigned. This object can't function without a NavMeshAgent!");

        player = GameObject.FindGameObjectWithTag("Player");
        currentStatus = Status.patroling;
        SetDestination();
    }

    private void Update()
    {
        // Check if this NPC's current status is partroling.
        if (currentStatus == Status.patroling)
        {
            _NMA.autoRepath = true;

            // Check if this object is close to the destionation
            if(_NMA.remainingDistance <= minDistFromPartolPoint)
            {
                _NMA.isStopped = true;
                isWaiting = true;
                stopWaitingAt = Time.time + waitTimes[currPartolPt]; // Must be done before currPartolPt is updated to new point
                Debug.Log(this.name + " has been set to isWaiting at: " + Time.time + ". This object will wait until " + stopWaitingAt + ", " + waitTimes[currPartolPt] + "sec after the current time.");

                NextPartrolPoint();
                SetDestination(partolPoints[currPartolPt].transform); // Set destionation to new partol point
            }

            if(isWaiting && stopWaitingAt < Time.time)
            {
                _NMA.isStopped = false;
                isWaiting = false;
            }
        }

        if(currentStatus == Status.idel)
        {
            _NMA.isStopped = true;
        }
    }

    private void NextPartrolPoint()
    {
        if (currPartolPt + 1 < partolPoints.Length)
            currPartolPt++;
        else
            currPartolPt = 0;
    }

    private void SetDestination()
    {
        _NMA.SetDestination(destination.transform.position);
    }

    private void SetDestination(Transform newDestination)
    {
        destination = newDestination;
        _NMA.SetDestination(destination.transform.position);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInSight = false;
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if(angle < fildOfViewAngle * 0.5f)
            {
                RaycastHit hit;

                if(Physics.Raycast(transform.position+Vector3.up/2,direction.normalized,out hit, sightDistance))
                {
                    if(hit.collider.gameObject == player)
                    {
                        playerInSight = true;
                    }
                }
                Debug.DrawRay(transform.position + transform.up, direction.normalized);
            }
        }
    }
}
