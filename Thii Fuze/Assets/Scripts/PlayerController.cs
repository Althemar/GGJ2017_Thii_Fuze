using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        toBomb,
        toStart
    };

    public float Speed;

    Rigidbody2D rb;

    Vector2 velocity;

    private Trace _trace;
    private PlayerState _playerState;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        _playerState = PlayerState.toBomb;
    }

    void Update()
    {
        Vector2 move;
        move.x = Input.GetAxis("HorizontalJ1") + Input.GetAxis("HorizontalJ2");
        move.y = Input.GetAxis("VerticalJ1") + Input.GetAxis("VerticalJ2");
        rb.velocity -= velocity;
        velocity = move * Speed * Time.deltaTime;
        rb.velocity += velocity;

        print(_trace.getIdTrace());

        if (_trace.getTraceState() == Trace.TraceState.drawing)
            _trace.addPoint(transform.position);
    }

    public void setTrace(Trace trace)
    {
        _trace = trace;
    }

    /*
    public PlayerState getPlayerState()
    {
        return _playerState;
    }

    public void setPlayerState(PlayerState playerState)
    {
        _playerState = playerState;
    }
    */
}
