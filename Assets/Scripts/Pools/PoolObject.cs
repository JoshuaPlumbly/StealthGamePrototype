using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObjects : MonoBehaviour
{
    public virtual bool OnSpawn()
    {
        // No Override
        return true;
    }
}
