using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public abstract class EventInfo
    {
        public string EventDescripion;
    }

    public class DebuEventInfo : EventInfo
    {
        public int verbosityLevel;
    }

    public class CallBackUp : EventInfo
    {
        public GameObject go;
    }
}
