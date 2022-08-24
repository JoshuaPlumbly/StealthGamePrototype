using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.StateMachine
{
    [CreateAssetMenu(menuName = "Agent/Decision/DecToReload")]
    public class DecToReload : AgentDecision
    {
        public override bool Decide(AgentFSM agent)
        {
            return agent.AgentStats.selectedWeapon.IsUnloaded();
        }
    }
}