using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Code.FSM
{
    public class AgentFSM : MonoBehaviour
    {
        [SerializeField] private AgentStats agentStats;
        [SerializeField] private NavMeshAgent navAgent;

        public AgentState currentState;
        public AgentState previousState;
        public AgentState remainState;
        private float stateStartTime;

        public float StateStartTime { get { return stateStartTime; } }
        public AgentStats AgentStats { get { return agentStats; } }
        public NavMeshAgent NavAgent{ get { return navAgent; } }

        protected virtual void Awake()
        {
            previousState = null;
            agentStats = GetComponent<AgentStats>();
            navAgent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Update()
        {
            currentState.UpdateState(this);
        }

        public virtual void TransitionToState(AgentState nextState)
        {
            if (nextState != remainState)
            {
                currentState.Exit(this);
                previousState = currentState;
                currentState = nextState;
                stateStartTime = Time.time;
                currentState.Enter(this);
            }
        }

        public bool CheckIfCountDownElaapsed(float duration)
        {
            return (Time.time - stateStartTime < duration);
        }
    }
}
