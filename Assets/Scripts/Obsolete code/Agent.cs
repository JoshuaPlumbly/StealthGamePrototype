using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {
    [SerializeField] float maxSpeed;
    [SerializeField] float maxAccel;
    [SerializeField] float orientation;
    [SerializeField] float rotation;
    [SerializeField] Vector3 velocity;
    [SerializeField] protected Steering steering;

    private void Start()
    {
        velocity = Vector3.zero;
        steering = new Steering();
    }

    public void SetSteering(Steering steering)
    {
        this.steering = steering;
    }

    public virtual void Update()
    {
        Vector3 displacement = velocity * Time.deltaTime;
        orientation += rotation * Time.deltaTime;

        // Limit orientaion to a range of 0 to 360.
        if (orientation < 0.0f)
            orientation += 360.0f;
        else if (orientation > 360.0f)
            orientation -= 360.0f;

        transform.Translate(displacement, Space.World);
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.up, orientation);
    }

    public virtual void LateUpdate()
    {
        velocity += steering.Linear * Time.deltaTime;
        rotation += steering.Angular * Time.deltaTime;

        if(velocity.magnitude>maxSpeed)
        {
            velocity.Normalize();
            velocity = velocity * maxSpeed;
        }

        if (steering.Angular == 0.0f)
            velocity = Vector3.zero;

        if (steering.Linear.sqrMagnitude == 0.0f)
            velocity = Vector3.zero;

        steering = new Steering();
    }
}
