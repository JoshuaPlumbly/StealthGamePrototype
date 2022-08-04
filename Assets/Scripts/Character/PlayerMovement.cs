using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    Camera cam;

    [SerializeField] private float moveSpeed = 6.0f;
    [SerializeField] private float drag = 1.0f;
    [SerializeField] private float sneakSpeedBuff = 0.6f;
    [SerializeField] private float sneakNosieBuff = 0.2f;
    [SerializeField] LayerMask walkableTerrain;
    [SerializeField] private Color footStepRanage = Color.blue;

    private Vector3 velocity;
    private float footStep;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        // Calculate drag.
        velocity = velocity * (1 - Time.deltaTime * drag);

        // Chanage position of object.
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    public void Move(Vector3 direction)
    {
        velocity = direction.normalized * moveSpeed;
        transform.rotation = Quaternion.LookRotation(direction);

        CheckFootStep();
    }

    void CheckFootStep()
    {
        Ray ray = new Ray(this.transform.position, -Vector3.up);
    }

    void RotateCharacter(Vector3 movementDirection)
    {
        Quaternion lookRotation = Quaternion.LookRotation(movementDirection);
        if (transform.rotation != lookRotation)
        {
            transform.rotation = lookRotation;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, footStep);
    }
}
