using UnityEngine;
using Assets.Code.StateMachine;

[System.Serializable]
public class CombatPhase : BaseState
{
    private AwarenessStateMachine _stateMachine;
    private float _combatTime;
    private float _timer;

    public CombatPhase(AwarenessStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _combatTime = _stateMachine.CombatTime;
    }

    public override void EnterState()
    {
        _combatTime = _stateMachine.CombatTime;
        _timer = _combatTime;

        _stateMachine.InvokeSetAwarenssTo(AwarenessStateMachine.Awareness.Detected);
        _stateMachine.InvokeSetUIStatsTo(_stateMachine.Detected);
        _stateMachine.InvokeSetUIVisibility(true);
        _stateMachine.InvokeSetProgressTo(1f);
    }

    public override void UpdateState()
    {
        _timer -= Time.deltaTime;
        _stateMachine.InvokeSetProgressTo(_timer / _combatTime);
    }
}