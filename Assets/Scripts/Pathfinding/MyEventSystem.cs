using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// https://www.youtube.com/watch?v=04wXkgfd9V8

namespace Assets.Code
{
    public class MyEventSystem : MonoBehaviour
    {
        private static MyEventSystem _instance;
        public static MyEventSystem Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<MyEventSystem>();
                }

                return _instance;
            }
        }

        public delegate void EventListener(EventInfo ei);
        Dictionary<System.Type, List<EventListener>> eventListeners;

        void OnEnable()
        {
            _instance = this;
        }

        public void RegisterListener<T>(System.Action<T> listener) where T : EventInfo
        {
            System.Type eventType = typeof(T);
            if (eventListeners == null)
            {
                eventListeners = new Dictionary<System.Type, List<EventListener>>();
            }

            if (eventListeners.ContainsKey(eventType) == false || eventListeners[eventType] == null)
            {
                eventListeners[eventType] = new List<EventListener>();
            }

            EventListener wrapper = (ei) => { listener((T)ei); };

            eventListeners[eventType].Add(wrapper);
        }

        public void UnregisterListener(string eventType, EventListener listener)
        {

        }

        public void RunEvent(EventInfo eventInfo)
        {
            System.Type trueEventInfoClass = eventInfo.GetType();
            if (eventListeners == null || eventListeners[trueEventInfoClass] == null)
            {
                return;
            }

            foreach (EventListener el in eventListeners[trueEventInfoClass])
            {
                el(eventInfo);
            }
        }
    }
}