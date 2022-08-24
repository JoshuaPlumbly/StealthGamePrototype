using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.StateMachine
{
    public abstract class AgentDecision : ScriptableObject
    {
        public abstract bool Decide(AgentFSM agent);
    }
}
