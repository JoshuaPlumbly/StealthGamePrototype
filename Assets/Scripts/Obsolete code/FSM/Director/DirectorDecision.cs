using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.StateMachine
{
    public abstract class DirectorDecision : ScriptableObject
    {

        public abstract bool Decide(DirectorFSM Director);
    }
}