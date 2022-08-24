using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.StateMachine;

namespace Assets.Code
{
    public class StateMachine<T>
    {
        public State<T> CurrentState;
        public T Owner;

        public StateMachine(T owner)
        {
            Owner = owner;
            CurrentState = null;
        }

        public void ChanageState(State<T> _newState)
        {
            if (CurrentState!=null)
                CurrentState.ExitState(Owner);
            CurrentState = _newState;
            CurrentState.EnterState(Owner);
        }

        public void Update()
        {
            if (CurrentState != null)
                CurrentState.UpdateState(Owner);
        }
    }
}