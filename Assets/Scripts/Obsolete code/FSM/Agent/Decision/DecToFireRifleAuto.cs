using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.StateMachine
{
    [CreateAssetMenu(menuName = "Agent/Decision/DecToFireRifleAuto")]
    public class DecToFireRifleAuto : AgentDecision
    {
        public override bool Decide(AgentFSM agent)
        {
            float targetDistance = Vector3.Magnitude(agent.AgentStats.target.position - agent.AgentStats.transform.position);

            if (targetDistance >= agent.AgentStats.minShootDist && targetDistance <= agent.AgentStats.maxShootDist)
                return true;
            else
                return false;
        }
    }
}