using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.StateMachine
{
    [CreateAssetMenu(menuName = "StateMachine/Action/ChaseTarget")]
    public class ActChaseTarget : AgentAction
    {
        public override void Act(AgentFSM agent)
        {
            ChaseTarget(agent);
        }

        public void ChaseTarget(AgentFSM agent)
        {
            agent.NavAgent.destination = agent.AgentStats.target.position;
        }
    }
}