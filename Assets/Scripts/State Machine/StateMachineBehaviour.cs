using UnityEngine;


namespace Assets.Code.StateMachine
{
    public abstract class StateMachineManager<T> : MonoBehaviour where T : IState
    {
        public T CurrentState { get; protected set; }

        private void Update()
        {
            if (CurrentState != null)
                CurrentState.UpdateState();
        }

        protected void SwitchTo(T newState)
        {
            if (!CurrentState.Equals(null))
                CurrentState.ExitState();

            CurrentState = newState;

            if (!CurrentState.Equals(null))
                CurrentState.EnterState();
        }

        public bool IsStateSetTo(T stateToCheck)
        {
            return CurrentState.Equals(stateToCheck);
        }
    }
}