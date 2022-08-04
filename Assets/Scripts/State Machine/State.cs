using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Code
{
    public abstract class State<T>
    {
        public abstract void EnterState(T _owener);
        public abstract void ExitState(T _owner);
        public abstract void UpdateState(T _owner);
    }
}