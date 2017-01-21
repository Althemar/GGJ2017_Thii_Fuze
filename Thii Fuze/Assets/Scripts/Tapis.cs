using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Tapis : MonoBehaviour {

    [Range(-1, 1)]
    public float Direction;
    public float MaxSpeed;

    Vector2 velocity;

	void Start () {
		
	}

	void Update () {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        velocity = new Vector2(0, MaxSpeed * Direction * Time.deltaTime);
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
