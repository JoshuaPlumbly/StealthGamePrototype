using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code;

public class Chase : State<GuardAgent>
{
    private float reloadAt;
    private float shootAt;
    private float shootUntil;

    public override void EnterState(GuardAgent owner)
    {
        owner.NavAgent.SetDestination(owner.Player.position);
        shootAt = Time.time + owner.ShootAt;
        shootUntil = shootAt + owner.ShootFor;
    }

    public override void ExitState(GuardAgent owner)
    {

    }

    public override void UpdateState(GuardAgent owner)
    {
        owner.NavAgent.SetDestination(owner.Player.position);

        // Fire at player.
        if (Time.time > shootAt && Time.time < shootUntil)
        {
            if (owner.CanSeePlayer() && !owner.Gun.IsUnloaded())
            {
                owner.Gun.transform.LookAt(owner.Player);
                owner.Gun.PrimaryUse();
            }
        }

        // Set new shoot at and shoot until times.
        if (Time.time > shootUntil)
        {
            shootAt = Time.time + owner.ShootAt;
            shootUntil = shootAt + owner.ShootFor;
        }

        // Reload when gun is empty.
        if (owner.Gun.IsUnloaded())
            owner.Gun.TriggerReload();
    }

}