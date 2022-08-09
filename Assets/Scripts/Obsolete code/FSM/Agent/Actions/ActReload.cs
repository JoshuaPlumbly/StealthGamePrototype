using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.FSM
{
    [CreateAssetMenu(menuName = "Agent/Action/ActReload")]
    public class ActReload : AgentAction
    {
        public override void Act(AgentFSM agent)
        {
            // Reload selected weapon.
            agent.AgentStats.selectedWeapon.TriggerReload();
        }
    }
}