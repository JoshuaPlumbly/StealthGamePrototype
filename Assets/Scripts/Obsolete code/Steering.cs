using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour
{
    private float angular;
    private Vector3 linear;

    #region encapsulate vars
    public float Angular
    {
        get
        {
            return angular;
        }

        set
        {
            angular = value;
        }
    }

    public Vector3 Linear
    {
        get
        {
            return linear;
        }

        set
        {
            linear = value;
        }
    }
    #endregion

    public Steering()
    {
        angular = 0.0f;
        linear = new Vector3();
    }


}
