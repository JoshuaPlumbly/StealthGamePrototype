using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AwarenessStateMachine))]
public class AwarenessUI : MonoBehaviour
{
    [SerializeField] private GameObject _hudGameObject;
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private Image _fillImage;

    private AwarenessStateMachine _awarenessStatus;

    private void Awake()
    {
        _awarenessStatus = GetComponent<AwarenessStateMachine>();
        _awarenessStatus.SetUIVisibility += _hudGameObject.SetActive;
        _awarenessStatus.SetProgressTo += SetFill;
        _awarenessStatus.SetUIStatsTo += SetUIStatus;
    }

    private void OnDestroy()
    {
        _awarenessStatus.SetUIVisibility -= _hudGameObject.SetActive;
        _awarenessStatus.SetProgressTo -= SetFill;
        _awarenessStatus.SetUIStatsTo -= SetUIStatus;
    }

    private void SetFill(float fill) { _fillImage.fillAmount = fill; }
   
    private void SetUIStatus(AwarenessUIStats status)
    {
        _textMesh.text = status.Header;
        _textMesh.color = status.Color;
        _fillImage.color = status.Color;
    }
}
