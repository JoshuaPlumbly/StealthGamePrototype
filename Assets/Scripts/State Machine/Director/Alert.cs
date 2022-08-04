using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code;

public class Alert : State<GuardAgentsDirector>
{
    private Alert owner;
    private float spawnAt;
    private bool backUpArrived;

    private bool backupArrived =false;

    public override void EnterState(GuardAgentsDirector _owener)
    {
        // Lighting
        _owener.MainLight.intensity = _owener.AlarmLightIntenity;
        _owener.MainLight.color = _owener.AlarmLightColour;

        // Maked guards case player.
        foreach (GameObject guard in _owener.GuardList)
        {
            GuardAgent guardAgent = guard.GetComponent<GuardAgent>();
            guardAgent.StateMachine.ChanageState(guardAgent.Chase);
        }

        spawnAt = Time.time + _owener.SpawnTime;
    }

    public override void ExitState(GuardAgentsDirector _owner)
    {

    }

    public override void UpdateState(GuardAgentsDirector _owner)
    {
        // Check if backup arrived.
        if (backUpArrived)
        {
            if (_owner.GuardList.Count < _owner.GuardAlertAmount)
            {
                GameObject newGuard = PoolManager.Instance.SpawnObjectReturn(_owner.GuardPrefab, _owner.SpawnPoints[1].position, _owner.SpawnPoints[1].rotation);
                GuardAgent guardAgent = newGuard.GetComponent<GuardAgent>();
                guardAgent.StateMachine.ChanageState(guardAgent.Chase);
            }
        }
        else
        {
            // Spawn after a amount of time.
            if (Time.time > spawnAt)
            {
                // Spawn new guard.
                GameObject newGuard = PoolManager.Instance.SpawnObjectReturn(_owner.GuardPrefab, _owner.SpawnPoints[1].position, _owner.SpawnPoints[1].rotation);
                GuardAgent guardAgent = newGuard.GetComponent<GuardAgent>();
                guardAgent.StateMachine.ChanageState(guardAgent.Chase);
                backUpArrived = true;
            }
        }
    }
}
