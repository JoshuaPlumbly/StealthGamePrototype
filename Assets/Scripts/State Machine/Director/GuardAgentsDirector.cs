using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code;
using UnityEngine.UI;

public class GuardAgentsDirector : MonoBehaviour
{
    public event System.Action EnterCombatPhase;
    public void BackupRequest() => EnterCombatPhase?.Invoke();

    public List<GuardAgent> Guards { get; private set; }

    private void Start()
    {
        GuardAgent[] guards = FindObjectsOfType<GuardAgent>();

        for (int i = 0; i < guards.Length; i++)
        {
            Guards.Add(guards[i]);
        }
    }

    public bool CanAGuardSeePlayer()
    {
        foreach (var guard in Guards)
        {
            if (guard.GuardPerception.CanSeePlayerLastResult)
                return true;
        }

        return false;
    }
}
