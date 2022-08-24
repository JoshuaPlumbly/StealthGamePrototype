using UnityEngine;
using Assets.Code.StateMachine;

[RequireComponent(typeof(GuardAgentsDirector))]
public class AwarenessStateMachine : StateMachineManager<IState>
{ 
    public enum Awareness { Undetected, Detected, Alert };

    [field: SerializeField] public AwarenessUIStats Detected { get; private set; } = new AwarenessUIStats("Detected", Color.red);
    [field: SerializeField] public AwarenessUIStats Combat { get; private set; } = new AwarenessUIStats("Combat", Color.red);
    [field: SerializeField] public float CombatTime { get; private set; } = 5f;


    public event System.Action<Awareness> SetAwarenessTo;
    public event System.Action<float> SetProgressTo;
    public event System.Action<AwarenessUIStats> SetUIStatsTo;
    public event System.Action<bool> SetUIVisibility;
    public event System.Action BackupRequested;

    public void InvokeSetAwarenssTo(Awareness awareness) { SetAwarenessTo?.Invoke(awareness); }
    public void InvokeSetProgressTo(float amount) { SetProgressTo?.Invoke(amount); }
    public void InvokeSetUIStatsTo(AwarenessUIStats UIStats) { SetUIStatsTo?.Invoke(UIStats); }
    public void InvokeSetUIVisibility(bool value) { SetUIVisibility?.Invoke(value); }
    public void BackupRequest() => BackupRequested?.Invoke();

    public Awareness EntityAwareness { get; private set; } = Awareness.Undetected;
    public Detected DetectedState { get; private set; }
    public CombatPhase CombatPhaseState { get; private set; }

    private State<AwarenessStateMachine> _currentState;
    private GuardAgentsDirector _director;

    private void Start()
    {
        DetectedState = new Detected();
        CombatPhaseState = new CombatPhase(this);
        _director = FindObjectOfType<GuardAgentsDirector>();
        
        if (_director != null)
            _director.EnterCombatPhase += () => SwitchTo(CombatPhaseState);
    }

    private void OnDestroy()
    {
        if (_director != null)
            _director.EnterCombatPhase -= () => SwitchTo(CombatPhaseState);
    }
}