using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.FSM
{
    [CreateAssetMenu(menuName = "StateMachine/Decision/HeardFootStep")]
    public class HeardFootStepDecision : AgentDecision
    {
        public override bool Decide(AgentFSM agent)
        {
            bool heardFootstep = CheckFootStep(agent);
            return heardFootstep;
        }

        private bool CheckFootStep(AgentFSM agent)
        {
            return false;
        }
    }
}