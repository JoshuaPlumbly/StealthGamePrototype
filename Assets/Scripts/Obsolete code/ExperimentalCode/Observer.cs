using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Observer : MonoBehaviour
{
    public abstract void OnNotify(object value, NotificationType notificationType);
}

public abstract class Subject : MonoBehaviour
{
    private List<Observer> _observers = new List<Observer>();

    public void Register(Observer observer)
    {
        _observers.Add(observer);
    }

    public void Notify(Object value, NotificationType notificationType)
    {
        foreach (var observer in _observers)
        {
            observer.OnNotify(value, notificationType);
        }
    }
}

public enum NotificationType
{
    unspecific = 0
}
