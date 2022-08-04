using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.FSM
{
    [CreateAssetMenu(menuName = "StateMachine/Action/FireShot")]
    public class ActFireRifleAuto : AgentAction
    {
        public override void Act(AgentFSM agent)
        {
            FireShot(agent);
        }

        public void FireShot(AgentFSM agent)
        {
            agent.AgentStats.selectedWeapon.FireTrigger();
        }
    }
}