using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
    [field: SerializeField] public Gun Gun { get; private set; }
    [field: SerializeField] public TimerBehaviour Timer { get; private set; }

    public int Kills { get; private set; }
    public int Alerts { get; private set; }

    private void Awake()
    {
        GuardAgent.Alerted += () => Alerts++;
        GuardAgent.Died += () => Kills++;
    }

    private void OnDestroy()
    {
        GuardAgent.Alerted -= () => Alerts++;
        GuardAgent.Died += () => Kills++;
    }

    public PlayResults CurrentProgress()
    {
        PlayResults playResults = new PlayResults();
        playResults.kills = Kills;
        playResults.alerts = Alerts;
        playResults.ammo = Gun.TotalAmmo();
        playResults.time = Timer.RemainingTime();

        return playResults;
    }
}