using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.StateMachine
{
    [CreateAssetMenu(menuName = "Agent/Action/ActPatrol")]
    public class ActPatrol : AgentAction
    {
        public override void Act(AgentFSM agent)
        {
            
        }

        public void Patrol(AgentFSM agent)
        {
            agent.NavAgent.destination = agent.AgentStats.wayPointList[agent.AgentStats.currentWayPoint].position;
            agent.NavAgent.isStopped = false;

            if (agent.NavAgent.remainingDistance <= agent.AgentStats.stopingDistance && !agent.NavAgent.pathPending)
            {
                agent.AgentStats.currentWayPoint = (agent.AgentStats.currentWayPoint + 1) % agent.AgentStats.wayPointList.Length;
            }
        }
    }
}