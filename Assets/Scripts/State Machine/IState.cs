namespace Assets.Code.StateMachine
{
    public interface IState
    {
        public virtual void UpdateState() { }
        public virtual void EnterState() { }
        public virtual void ExitState() { }
    }
}