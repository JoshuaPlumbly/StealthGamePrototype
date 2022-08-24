using Assets.Code.StateMachine;

public class CallForBackup : State<GuardAgent>
{
    float BackupCalledAt;

    public override void EnterState(GuardAgent owner)
    {
        owner.NavAgent.SetDestination(owner.FindNearestAlarm());
        owner.NavAgent.Play();
    }


    public override void UpdateState(GuardAgent owner)
    {
        if(owner.NavAgent.RemainingDistance() < owner.AlarmPullDst)
        {
            owner.StateMachine.ChanageState(owner.Chase);
            owner.GuardAgentsDirector.BackupRequest();
        }
    }
}