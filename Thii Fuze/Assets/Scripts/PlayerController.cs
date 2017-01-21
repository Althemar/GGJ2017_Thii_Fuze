﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    public GameObject arrowJ1;
    public GameObject arrowJ2;

    public float arrowDistance = 1f;

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
        Vector2 moveJ1;
        Vector2 moveJ2;
        moveJ1 = new Vector2(Input.GetAxis("HorizontalJ1"), Input.GetAxis("VerticalJ1"));
        moveJ2 = new Vector2(Input.GetAxis("HorizontalJ2"), Input.GetAxis("VerticalJ2"));
        //arrowJ1.transform.localPosition = moveJ1 * arrowDistance;
        //arrowJ2.transform.localPosition = moveJ2 * arrowDistance;
        Vector2 move = moveJ1 + moveJ2;
        rb.velocity -= velocity;
        velocity = move * Speed * Time.deltaTime;
        rb.velocity += velocity;

        if (_playerState == PlayerState.toBomb)
            _trace.addPoint(transform.position);
        /*
        try
        {
            if (_playerState == PlayerState.toBomb)
                _trace.addPoint(transform.position);
        }
        catch
        {
            Debug.LogWarning("Trace system not setup in scene.");
        }
        */

    }

    public void setTrace(Trace trace)
    {
        _trace = trace;
    }

    
    public PlayerState getPlayerState()
    {
        return _playerState;
    }

    public void setPlayerState(PlayerState playerState)
    {
        _playerState = playerState;
    }
    
}
