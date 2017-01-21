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

    void LateUpdate()
    {
        Vector2 move;
        rb.velocity -= velocity;
        move.x = Input.GetAxis("HorizontalJ1") + Input.GetAxis("HorizontalJ2");
        move.y = Input.GetAxis("VerticalJ1") + Input.GetAxis("VerticalJ2");
        velocity = move * Speed * Time.deltaTime;
        rb.velocity += velocity;

        try
        {
            if (_trace.getTraceState() == Trace.TraceState.drawing)
                _trace.addPoint(transform.position);
        }
        catch
        {
            Debug.Log("Trace system not setup in scene.");
        }
    }
}
