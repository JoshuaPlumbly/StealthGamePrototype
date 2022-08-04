using UnityEngine;
using Text = UnityEngine.UI.Text;
using TimeSpan = System.TimeSpan;

[RequireComponent(typeof(TimerBehaviour))]
public class UITimerBehaviour : MonoBehaviour
{
    [SerializeField] private Text _timerText;

    private TimerBehaviour _timerBehaviour;

    private void Awake()
    {
        _timerBehaviour = GetComponent<TimerBehaviour>();
    }

    private void Update()
    {
        var timeSpan = TimeSpan.FromSeconds(_timerBehaviour.RemainingTime());
        _timerText.text = timeSpan.ToString("mm':'ss':'fff");
    }
}