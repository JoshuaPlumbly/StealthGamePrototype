using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code;

public class Death : State<GuardAgent>
{
    private static Death _instance;
    public static Death Instance
    {
        get
        {
            if (_instance == null)
            {
                new Death();
            }

            return _instance;
        }
    }

    public Death()
    {
        if (_instance!=null)
        {
            return;
        }

        _instance = this;
    }

    float despawnAt;

    public override void EnterState(GuardAgent _owner)
    {
        // Stop moving and fall down.
        _owner.NavAgent.StopMoving();
        _owner.RididBody.freezeRotation = false;
        _owner.transform.rotation *= Quaternion.Euler(Random.insideUnitSphere.normalized * 20.0f);
        _owner.RididBody.AddForce(Vector3.forward * Random.Range(12f, 16f));

        // Set amount of secound until despawn.
        despawnAt = Time.time + _owner.DespawnTime;
    }

    public override void ExitState(GuardAgent _owner)
    {
        
    }

    public override void UpdateState(GuardAgent _owner)
    {
        // Despawn after a amount of time.
        if (Time.time > despawnAt)
        {
            _owner.gameObject.SetActive(false);
        }
    }

}