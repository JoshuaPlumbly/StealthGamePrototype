using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag == "Player")
        {
            LevelCompleation.InvokeLevelCompleated();
        }
    }
}
