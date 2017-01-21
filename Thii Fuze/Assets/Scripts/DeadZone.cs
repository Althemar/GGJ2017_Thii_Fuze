using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DeadZone : MonoBehaviour {

    public delegate void DeadZoneEvent();
    public static event DeadZoneEvent OnPlayerEnterDeadZone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayerEnterDeadZone();
        }
    }

}
