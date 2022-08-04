using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticRifle : Gun
{
    public override void FireTrigger()
    {
        if (Time.time > timeLastFiredAt && loadedAmmo > 0)
        {
            timeLastFiredAt = Time.time + secPerRound;
            loadedAmmo--;
            Shoot();
        }
    }
}
