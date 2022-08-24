using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.StateMachine
{
    public abstract class DirectorAction : ScriptableObject
    {
        public abstract void Act(DirectorFSM Director);
    }
}