using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bomb : MonoBehaviour {

    public delegate void BombEvent();
    public static event BombEvent OnPlayerInitiateBomb;
    public static event BombEvent OnBombExplosion;


    bool bombInitiated = false;

    private void Awake()
    {
        OnPlayerInitiateBomb += initiateBomb;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !bombInitiated)
        {
            Debug.Log("OnPlayerInitiateBomb");
            OnPlayerInitiateBomb();
        }
    }

    void initiateBomb()
    {
        bombInitiated = true;
    }
}
