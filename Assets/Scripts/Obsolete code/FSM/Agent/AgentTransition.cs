using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.StateMachine
{
    [System.Serializable]
    public class AgentTransition
    {
        public AgentDecision decision;
        public AgentState trueState;
        public AgentState falseState;
    }
}