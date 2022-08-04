using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.FSM
{
    [System.Serializable]
    public class DirectorTransition
    {
        public DirectorDecision decision;
        public DirectorState trueState;
        public DirectorState falseState;
    }
}