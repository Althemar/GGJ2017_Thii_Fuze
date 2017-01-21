using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    public float Speed;

    Rigidbody2D rb;

    Vector2 velocity;

    private Trace _trace;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        _trace = GetComponent<Trace>();
    }

    void Update()
    {
        Vector2 move;
        move.x = Input.GetAxis("HorizontalJ1") + Input.GetAxis("HorizontalJ2");
        move.y = Input.GetAxis("VerticalJ1") + Input.GetAxis("VerticalJ2");
        rb.velocity -= velocity;
        velocity = move * Speed * Time.deltaTime;
        rb.velocity += velocity;

        if (_trace.getTraceState() == Trace.TraceState.drawing)
            _trace.addPoint(transform.position);
    }
}
