using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class NavUnit : MonoBehaviour
{
    public const float _minPathUpdateTime = 0.2f;
    public const float _pathUpdateMoveThreshold = 0.5f;

    public Vector3 Destination { get; private set; }
    public float Speed { get; set; } = 7.0f;
    public float TurnSpeed { get; set; } = 10f;
    public float TurnDst { get; set; } = 0.1f;
    public float TargetAccuracy { get; set; }
    public float StoppingDst { get; set; } = 10.0f;
    public float RemainingDistance { get; private set; }

    [SerializeField] private bool _drawPath;

    private Path _path;
    private Rigidbody _ridigbody;

    public void SetDestination(Vector3 destination)
    {
        Destination = destination;
    }

    public void SetDestination(Transform destination)
    {
        SetDestination(destination.position);
    }

    private void Awake()
    {
        _ridigbody = GetComponent<Rigidbody>();
    }

    public void StartMoving()
    {
        StartCoroutine(UpdatePath_FixedDestination());
    }

    public void StopMoving()
    {
        StopCoroutine(UpdatePath_FixedDestination());
        StopCoroutine(FollowPath());
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            // Create a new path using a list of waypoints.
            _path = new Path(waypoints, transform.position, TurnDst, StoppingDst);

            // Start moving towards the destination.
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator UpdatePath_FixedDestination()
    {
        // Make sure that time sine the level loaded is over 0.3 secounds.
        if (Time.timeSinceLevelLoad < 0.3f)
        {
            yield return new WaitForSeconds(0.3f);
        }

        // Use the path request manager to find a new path to the destation.
        PathRequestManager.RequestPath(new PathRequest(transform.position, Destination, OnPathFound));

        // Create variables for while loop.
        float sqrMoveThreshold = _pathUpdateMoveThreshold * _pathUpdateMoveThreshold;
        Vector3 targetPosOld = Destination;

        while (true)
        {
            // Only update after a certain amount of time.
            yield return new WaitForSeconds(_minPathUpdateTime);

            // Only move when destaination has moved.
            if ((Destination - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, Destination, OnPathFound));
                targetPosOld = Destination;
            }
        }
    }

    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(_path.lookPoints[0]);

        float speedPercent = 1;

        while (followingPath)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);

            // Run when object has reached a waypoint.
            while (_path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                // Find out if object has reached the last waypoint.
                if (pathIndex == _path.finishLineIndex)
                {
                    // Stop following path.
                    followingPath = false;
                    break;
                }
                else
                {
                    // Chanage destaination to next waypoint.
                    pathIndex++;
                }
            }

            if (followingPath)
            {
                // Calulate distance to next point.
                float dstToEndPoint = Vector3.Distance(transform.position, _path.lookPoints[pathIndex]);

                // Calulate distance to end point.
                for (int i = pathIndex; i < _path.lookPoints.Length - 1; i++)
                {
                    dstToEndPoint += Vector3.Distance(_path.lookPoints[i], _path.lookPoints[i + 1]);
                }

                RemainingDistance = dstToEndPoint;

                // Slow down when close to the target.
                if (pathIndex >= _path.slowDownIndex && StoppingDst > 0)
                {
                    speedPercent = Mathf.Clamp01(_path.turnBoundaries[_path.finishLineIndex].DistanceFromPoint(pos2D) / StoppingDst);
                    if (dstToEndPoint < TargetAccuracy)
                    {
                        followingPath = false;
                    }
                }

                // Move towards waypoint.
                //Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                Quaternion targetRotation = Quaternion.LookRotation(_path.lookPoints[pathIndex] - transform.position);
                targetRotation.x = 0.0f;
                targetRotation.z = 0.0f;
                _ridigbody.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * TurnSpeed);
                _ridigbody.MovePosition(_ridigbody.position + transform.forward * Time.deltaTime * Speed * speedPercent);
            }

            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (_drawPath = _path != null)
        {
            _path.DrawWithGizmos();
        }
    }
}