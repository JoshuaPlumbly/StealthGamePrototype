using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decision : DecisionTreeNode
{
    public Action nodeTrue;
    public Action nodeFalse;

    public virtual Action GetBrach()
    {
        return null;
    }
}
