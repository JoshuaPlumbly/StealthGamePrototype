using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _camera;
    private void Awake()
    {
        _camera = Camera.main.transform;
        transform.rotation = Quaternion.LookRotation(transform.position-_camera.position);
    }

    private void Update()
    {
        transform.forward = _camera.forward;
    }
}