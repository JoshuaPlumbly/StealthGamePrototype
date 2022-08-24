using System.Collections;
using System.Collections.Generic;


namespace Assets.Code.StateMachine
{
    public abstract class State<T>
    {
        public virtual void EnterState(T owener) { }
        public virtual void ExitState(T owner) { }
        public virtual void UpdateState(T owner) { }
    }
}