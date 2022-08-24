using Assets.Code.StateMachine;

public class Detected : State<AwarenessStateMachine>
{
    public override void EnterState(AwarenessStateMachine owener)
    {
        owener.InvokeSetAwarenssTo(AwarenessStateMachine.Awareness.Detected);
        owener.InvokeSetUIStatsTo(owener.Detected);
        owener.InvokeSetProgressTo(1);
        owener.InvokeSetUIVisibility(true);
    }
}