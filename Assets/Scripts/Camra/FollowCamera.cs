using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Vector3 offsetPos = new Vector3(0.0f, 6.0f, -8.0f);
    [SerializeField] private Vector3 offsetRot = new Vector3(30.0f, 0.0f, 0.0f);
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 8.0f;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        transform.rotation = Quaternion.Euler(offsetRot);
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.transform.position + offsetPos;
        Vector3 newPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = newPosition;
    }
}