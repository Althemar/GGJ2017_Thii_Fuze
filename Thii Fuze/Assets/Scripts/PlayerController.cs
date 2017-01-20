using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    public float Speed;

    Rigidbody2D rb;

    Vector2 velocity;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void LateUpdate () {
        Vector2 move;
        rb.velocity -= velocity;
        move.x = Input.GetAxis("HorizontalJ1") + Input.GetAxis("HorizontalJ2");
        move.y = Input.GetAxis("VerticalJ1") + Input.GetAxis("VerticalJ2");
        velocity = move * Speed * Time.deltaTime;
        rb.AddForce(move * Speed * Time.deltaTime/2);
        rb.velocity += velocity;
	}
}
