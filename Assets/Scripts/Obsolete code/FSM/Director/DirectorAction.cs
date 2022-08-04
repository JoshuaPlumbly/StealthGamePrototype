using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.FSM
{
    public abstract class DirectorAction : ScriptableObject
    {
        public abstract void Act(DirectorFSM Director);
    }
}