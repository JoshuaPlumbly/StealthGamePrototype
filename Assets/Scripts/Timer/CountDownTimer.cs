using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CountDownTimer
{
    public float RemainingTime { get; private set; }
    public bool HasCompleated { get; private set; } = false;

    public event System.Action Compleated;

    public CountDownTimer(float duration)
    {
        RemainingTime = duration;
    }

    public CountDownTimer(float timeLeft, bool hasCompleated) : this(timeLeft)
    {
        HasCompleated = hasCompleated;
    }

    public void Reduce(float duration)
    {
        if (HasCompleated)
            return;

        RemainingTime -= duration;

        if (RemainingTime <= 0)
        {
            RemainingTime = 0;
            Compleated?.Invoke();
            HasCompleated = true;
        }
    }

    public void Set(float duration)
    {
        RemainingTime = duration;
    }
}