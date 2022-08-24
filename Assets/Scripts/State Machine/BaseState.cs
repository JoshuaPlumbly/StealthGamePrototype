namespace Assets.Code.StateMachine
{
    public abstract class BaseState : IState
    {
        public virtual void UpdateState() { }
        public virtual void EnterState() { }
        public virtual void Exit() { }
    }}