using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WinZone : MonoBehaviour {

    public delegate void WinZoneEvent();
    public static event WinZoneEvent OnPlayerEnterWinZone;

    private void Awake()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        Bomb.OnPlayerInitiateBomb += activateZone;
    }

    public void activateZone()
    {
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(OnPlayerEnterWinZone != null)
            {
                Debug.Log("OnPlayerEnterWinZone");
                OnPlayerEnterWinZone();
            }
        }
    }

}
