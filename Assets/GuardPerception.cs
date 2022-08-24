using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPerception : MonoBehaviour
{
    [SerializeField] private Transform _visionOrigin;
    [SerializeField] private float _sightDistance = 6f;
    [SerializeField] private float _sightAngleDOT = 0.38f;

    public event System.Action<bool> CanSeePlayer;

    public bool CanSeePlayerLastResult { get; private set; } = false;

    private Transform _playerTransform;
    private GuardAgentsDirector _director;

    private void Awake()
    {
        _playerTransform = FindObjectOfType<PlayerMovement>().transform;
        _director = FindObjectOfType<GuardAgentsDirector>();
    }

    private void Update()
    {
        bool canSeePlayer = CanSeePlayerCheck();

        if (CanSeePlayerLastResult != canSeePlayer)
        {
            CanSeePlayerLastResult = canSeePlayer;
            CanSeePlayer?.Invoke(canSeePlayer);
        }
    }

    public bool CanSeePlayerCheck()
    {
        Vector3 toPlayer = _playerTransform.position - _visionOrigin.position;
        Ray ray = new Ray(_visionOrigin.position, toPlayer);

        if (!Physics.Raycast(ray, out var hit, _sightDistance))
            return false;

        if (hit.transform.root != _playerTransform)
            return false;

        float dot = Vector3.Dot(_visionOrigin.forward, toPlayer.normalized);

        return dot < _sightAngleDOT;
    }
}