using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code;

public class CallForBackup : State<GuardAgent>
{
    float BackupCalledAt;

    public CallForBackup()
    {

    }

    public override void EnterState(GuardAgent owner)
    {
        owner.NavAgent.SetDestination(owner.FindNearestAlarm());
        owner.NavAgent.StartMoving();
    }

    public override void ExitState(GuardAgent owner)
    {

    }

    public override void UpdateState(GuardAgent owner)
    {
        if(owner.NavAgent.RemainingDistance < owner.AlarmPullDst)
        {
            owner.StateMachine.ChanageState(owner.Chase);
            GuardAgentsDirector.BackupRequest();
        }
    }

}