using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code;
using Assets.Code.StateMachine;

[System.Serializable]
public class Patrol : State<GuardAgent>
{
    public int _patrolPointIndex;
    public Transform player;
    public float timeAt;

    //public static event System.Action<GuardAgent> Detected;

    public override void EnterState(GuardAgent owner)
    {
        owner.NavAgent.SetDestination(owner.PatrolPoints[0]);
        owner.NavAgent.Speed = owner.PatrollingWalkSpeed;
        owner.NavAgent.Play();

        _patrolPointIndex = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void ExitState(GuardAgent owner)
    {
        owner.NavAgent.Pause();
    }

    public override void UpdateState(GuardAgent owner)
    {
        // Check if agent has reached the current waypoint.
        if (owner.NavAgent.ReachedWaypoint())
        {
            _patrolPointIndex = (_patrolPointIndex + 1) % owner.PatrolPoints.Length;
            owner.NavAgent.SetDestination(owner.PatrolPoints[_patrolPointIndex].position);
            owner.NavAgent.Play();
        }

        //if (owner.CanSeePlayer)
        //{
        //    owner.StateMachine.ChanageState(owner.CallForBackup);
        //}
    }
}