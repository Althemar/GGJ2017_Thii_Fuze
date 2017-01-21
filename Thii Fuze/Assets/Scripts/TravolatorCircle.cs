using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TravolatorCircle : MonoBehaviour {

    private IDirectionalArea _controller;
    public InteriorCollider ic;

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
        Vector2 v = transform.position - collision.transform.position;

        Vector2 P3 = new Vector2(-v.y, v.x) / Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2)) * v.magnitude;

        Vector2 P4 = new Vector2(-v.y, v.x) / Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2)) * -v.magnitude;

        velocity = P3 * MaxSpeed * Direction * Time.deltaTime;
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        rb.velocity += velocity;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!ic.isIn)
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity -= velocity;

            Vector2 v = transform.position - collision.transform.position;

            Vector2 P3 = new Vector2(-v.y, v.x) / Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2)) * v.magnitude;

            Vector2 P4 = new Vector2(-v.y, v.x) / Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2)) * -v.magnitude;

            velocity = P3 * MaxSpeed * Direction * Time.deltaTime;
            rb.velocity += velocity;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        rb.velocity -= velocity;
    }

}
