using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code;

public class PlayerWin : State<GuardAgentsDirector>
{
    private static PlayerWin _instance;
    public static PlayerWin Instance
    {
        get
        {
            if (_instance == null)
            {
                new PlayerWin();
            }

            return _instance;
        }
    }

    private GuardAgentsDirector owner;

    public PlayerWin()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = this;
    }

    public override void EnterState(GuardAgentsDirector _owener)
    {
        Debug.Log("Player has reached goal.");

        // Disable guards
        foreach (GameObject guards in _owener.GuardList)
        {
            guards.SetActive(false);
        }
    }

    public override void ExitState(GuardAgentsDirector _owner)
    {

    }

    public override void UpdateState(GuardAgentsDirector _owner)
    {
        
    }
}
