using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorCollider : MonoBehaviour {

    public bool isIn;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isIn = false;
        }
    }


}
