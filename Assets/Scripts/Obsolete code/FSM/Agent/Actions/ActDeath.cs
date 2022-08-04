using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.FSM
{
    [CreateAssetMenu(menuName = "StateMachine/Action/Die")]
    public class ActDeath : AgentAction
    {
        public override void Act(AgentFSM agent)
        {
            Patrol(agent);
        }

        public void Patrol(AgentFSM agent)
        {
            agent.NavAgent.isStopped = true;
        }
    }
}