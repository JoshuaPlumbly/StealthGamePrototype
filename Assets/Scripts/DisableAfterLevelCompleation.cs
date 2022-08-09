using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterLevelCompleation : MonoBehaviour
{
    private void Awake()
    {
        LevelCompleation.LevelCompleated += () => gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        LevelCompleation.LevelCompleated -= () => gameObject.SetActive(false);
    }
}
