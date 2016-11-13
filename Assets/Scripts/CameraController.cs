using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Vector3 offsetPosition = new Vector3(0f, 20f, -10f);

    Transform _target;

    void Awake()
    {
        _target = GameObject.FindObjectOfType<PlayerManager>().transform;
    }

    void Start()
    {
        UpdatePosition();
        transform.LookAt(_target);
    }

    void UpdatePosition()
    {
        transform.position = _target.position + offsetPosition;
    }

    void LateUpdate()
    {
        if (_target != null)
        {
            UpdatePosition();
        }    
    }
}
