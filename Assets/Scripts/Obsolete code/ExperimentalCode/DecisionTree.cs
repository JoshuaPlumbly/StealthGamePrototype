using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree : DecisionTreeNode
{
    public DecisionTreeNode root;
    private Action actionNew;
    private Action actionOld;

    public override DecisionTreeNode MakeDecision()
    {
        return root.MakeDecision();
    }

    private void Update()
    {
        actionNew.activated = false;
        actionOld = actionNew;
        actionNew = root.MakeDecision() as Action;
        if (actionNew == null)
            actionNew = actionOld;

        actionNew.activated = true;
    }
}
