using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Travolator : MonoBehaviour {

    private IDirectionalArea _controller;

    //[Range(-1, 1)]
    public float Direction
    {
        get
        {
            return _controller.GetDirection();
        }
    }

    public float MaxSpeed
    {
        get
        {
            const float debug_multiplicator = 400;
            return _controller.GetVelocity() * debug_multiplicator;
        }
    }

    Vector2 velocity;

    void Start()
    {
        _controller = GetComponent<IDirectionalArea>();
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        velocity = new Vector2(0, MaxSpeed * Direction * Time.deltaTime);
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        rb.velocity += velocity;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        rb.velocity -= velocity;
        velocity = new Vector2(0, MaxSpeed * Direction * Time.deltaTime);
        rb.velocity += velocity;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        rb.velocity -= velocity;
    }

}
