using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public delegate void GameOverEvent();
    public static event GameOverEvent OnGameOver;

    public delegate void GameWonEvent();
    public static event GameWonEvent OnGameWon;

    private void Awake()
    {
        WinZone.OnPlayerEnterWinZone += gameWon;
        Bomb.OnBombExplosion += gameLost;
        DeadZone.OnPlayerEnterDeadZone += gameLost;
    }

    void gameWon()
    {
        OnGameWon();
    }

    void gameLost()
    {
        OnGameOver();
    }

    void Start () {
		
	}
	
	void Update () {
		
	}
}
