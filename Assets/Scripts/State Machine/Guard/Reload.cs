using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code;

public class Reload : State<GuardAgent>
{
    private static Reload _instance;
    public static Reload Instance
    {
        get
        {
            if (_instance == null)
            {
                new Reload();
            }

            return _instance;
        }
    }

    public Reload()
    {
        if (_instance!=null)
        {
            return;
        }

        _instance = this;
    }

    public override void EnterState(GuardAgent _owener)
    {

    }

    public override void ExitState(GuardAgent _owner)
    {

    }

    public override void UpdateState(GuardAgent _owner)
    {

    }

}