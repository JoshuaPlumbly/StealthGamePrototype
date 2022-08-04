using UnityEngine;

public class TimerBehaviour : MonoBehaviour
{
    [SerializeField] private float _duration = 1f;
    private CountDownTimer _timer;

    public event System.Action TimerCompleated;

    private void Start()
    {
        _timer = new CountDownTimer(_duration);
        _timer.Compleated += TimerCompleated;
    }

    private void Update()
    {
        _timer.Reduce(Time.deltaTime);
    }

    public float RemainingTime() { return _timer.RemainingTime; }
}
