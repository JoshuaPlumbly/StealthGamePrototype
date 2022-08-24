using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class NavUnit : MonoBehaviour
{
    public const float _minPathUpdateTime = 0.2f;
    public const float _pathUpdateMoveThreshold = 0.5f;
    public float _stopingDistance = 1f;

    public Vector3 Destination { get; private set; }
    public float Speed = 7.0f;
    public float AngularSpeed = 10f;
    public float TurnDistance = 0.1f;
    public float TargetAccuracy;
    public float StoppingDistance = 10.0f;

    [SerializeField] private bool _drawPath;

    private Path _path;
    private Rigidbody _ridigbody;
    private Vector3 _currentWaypoint;
    private int _currentWaypointIndex;
    private bool _isPlaying;
    private bool _hasPath;

    private void Awake()
    {
        _ridigbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_isPlaying)
            return;

        MoveAlongPathToDestination();
    }

    public void SetDestination(Vector3 destination)
    {
        if (destination == null)
        {
            Debug.LogWarning($"{name} : SetDestination was given a null Vector3.");
            return;
        }

        Destination = destination;
    }

    public void SetDestination(Transform destination)
    {
        if (destination == null)
        {
            Debug.LogWarning($"{name} : SetDestination was given a null Transform.");
            return;
        }

        SetDestination(destination.position);
    }

    public void Play()
    {
        _isPlaying = true;
        StartCoroutine(FindPathToDestination());
    }

    public void Pause()
    {
        _isPlaying = false;
        StopCoroutine(FindPathToDestination());
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if (!pathSuccessful)
            return;
        
        // Create a new path using a list of waypoints.
        _path = new Path(waypoints, transform.position, TurnDistance, StoppingDistance);
        SetWaypoint(0);

        //// Start moving towards the destination.
        //StopCoroutine("FollowPath");
        //StartCoroutine("FollowPath");
    }

    IEnumerator FindPathToDestination()
    {
        // Make sure that time sine the level loaded is over 0.3 secounds.
        if (Time.timeSinceLevelLoad < 0.3f)
        {
            yield return new WaitForSeconds(0.3f);
        }

        // Use the path request manager to find a new path to the destation.
        PathRequestManager.RequestPath(new PathRequest(transform.position, Destination, OnPathFound));
    }

    private void MoveAlongPathToDestination()
    {
        if (_currentWaypoint.Equals(null))
            return;

        if (ReachedWaypoint())
        {
            if (IsFollowingLastWaypoint())
                Pause();
            else
                SetWaypoint(_currentWaypointIndex + 1);
        }

        Vector3 targetDirection = _currentWaypoint - transform.position;
        targetDirection.y = 0;
        Vector3.Normalize(targetDirection);

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * AngularSpeed);

        _ridigbody.MoveRotation(rotation);
        _ridigbody.MovePosition(transform.position + (transform.forward * Time.deltaTime * Speed));
    }

    private void SetWaypoint(int pathIndex)
    {
        if (pathIndex < 0 || pathIndex >= _path.WaypointCount)
        {
            Debug.LogWarning($"{name} waypoint has been set to an out of range number");
            return;
        }

        _currentWaypointIndex = pathIndex;
        _currentWaypoint = _path._wayPoints[pathIndex];
    }

    public bool IsFollowingLastWaypoint()
    {
        if (_path.Equals(null))
            return false;

        return _path.WaypointCount == _currentWaypointIndex;
    }

    public float DistanceToCurrentWaypoint()
    {
        return Vector3.Distance(transform.position, _currentWaypoint);
    }

    public float RemainingDistance() {

        // Calulate distance to next point.
        float remainingDistance = DistanceToCurrentWaypoint();

        // Calulate distance to end point.
        for (int i = _currentWaypointIndex; i < _path._wayPoints.Length - 1; i++)
        {
            remainingDistance += Vector3.Distance(_path._wayPoints[i], _path._wayPoints[i + 1]);
        }

        return remainingDistance;
    }

    public bool ReachedWaypoint()
    {
        if (_path == null || _path.WaypointCount == 0)
            return false;

        return DistanceToCurrentWaypoint() < _stopingDistance;
    }

    public void OnDrawGizmos()
    {
        if (_drawPath && _path != null)
        {
            _path.DrawWithGizmos();
        }
    }
}