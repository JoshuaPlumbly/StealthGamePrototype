using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : DecisionTreeNode
{
    public bool activated = false;

    public override DecisionTreeNode MakeDecision()
    {
        return this;
    }

    public virtual void LastUpdate()
    {
        if (!activated)
            return;
    }
}
