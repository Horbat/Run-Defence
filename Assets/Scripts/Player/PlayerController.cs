using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 moveVelocity;
    private float inertia = .1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

        moveVelocity = Vector3.Lerp(moveVelocity, Vector3.zero, Time.fixedDeltaTime / inertia);
    }

    public void Move(Vector3 velocity)
    {
        moveVelocity = velocity;
    }

    public void Look(Vector2 direction)
    {
        Vector3 target = transform.position + new Vector3(direction.x, 0f, direction.y);
        transform.LookAt(target);
    }
}
