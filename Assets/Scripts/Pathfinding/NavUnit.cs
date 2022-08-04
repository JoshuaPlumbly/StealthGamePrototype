using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class NavUnit : MonoBehaviour
{
    public const float minPathUpdateTime = 0.2f;
    public const float pathUpdateMoveThreshold = 0.5f;

    public Vector3 Destination { get; private set; }
    public float Speed { get; set; } = 7.0f;
    public float TurnSpeed { get;  set; } = 10f;
    public float TurnDst { get;  set; } = 0.1f;
    public float TargetAccuracy { get;  set; }
    public float StoppingDst { get;  set; } = 10.0f;
    public float RemainingDistance { get; private set; }

    [SerializeField] private bool _drawPath;

    private Path path;
    private Rigidbody rb;

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
        rb = GetComponent<Rigidbody>();
    }

    public void StartMoving()
    {
        StartCoroutine(UpdatePath());
    }

    public void StopMoving()
    {
        StopCoroutine(UpdatePath());
        StopCoroutine(FollowPath());
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            // Create a new path using a list of waypoints.
            path = new Path(waypoints, transform.position, TurnDst, StoppingDst);

            // Start moving towards the destination.
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator UpdatePath()
    {
        // Make sure that time sine the level loaded is over 0.3 secounds.
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }

        // Use the path request manager to find a new path to the destation.
        PathRequestManager.RequestPath(new PathRequest(transform.position, Destination, OnPathFound));

        // Create variables for while loop.
        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = Destination;

        while (true)
        {
            // Only update after a certain amount of time.
            yield return new WaitForSeconds(minPathUpdateTime);

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
        transform.LookAt(path.lookPoints[0]);

        float speedPercent = 1;

        while (followingPath)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);

            // Run when object has reached a waypoint.
            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                // Find out if object has reached the last waypoint.
                if (pathIndex == path.finishLineIndex)
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
                float dstToEndPoint = Vector3.Distance(transform.position, path.lookPoints[pathIndex]);

                // Calulate distance to end point.
                for (int i = pathIndex; i < path.lookPoints.Length - 1; i++)
                {
                    dstToEndPoint += Vector3.Distance(path.lookPoints[i], path.lookPoints[i + 1]);
                }

                RemainingDistance = dstToEndPoint;

                // Slow down when close to the target.
                if (pathIndex >= path.slowDownIndex && StoppingDst > 0)
                {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / StoppingDst);
                    if (dstToEndPoint < TargetAccuracy)
                    {
                        followingPath = false;
                    }
                }

                // Move towards waypoint.
                //Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                targetRotation.x = 0.0f;
                targetRotation.z = 0.0f;
                rb.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * TurnSpeed);
                rb.MovePosition(rb.position + transform.forward * Time.deltaTime * Speed * speedPercent);
            }

            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (_drawPath = path != null)
        {
            path.DrawWithGizmos();
        }
    }
}