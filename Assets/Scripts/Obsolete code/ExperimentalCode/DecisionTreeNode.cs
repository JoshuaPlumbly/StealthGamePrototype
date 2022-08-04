using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTreeNode : MonoBehaviour
{
    public virtual DecisionTreeNode MakeDecision()
    {
        return null;
    }
}
