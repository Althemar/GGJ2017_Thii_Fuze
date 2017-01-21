using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public string level = "Level {0}";
    public string winText = "Well done ! Next Level !";
    public string lostText = "Game Over !";
    public string replay = "Press A to play again.";
    public string resume = "Press A to resume the game.";


    public Text finalText;
    public Text replayText;
    public Text levelText;

    Animator animator;

    void Awake ()
    {
        replayText.text = replay;
        animator = gameObject.GetComponent<Animator>();
        GameController.OnGameOver += displayLostMessage;
        GameController.OnGameWon += displayWinMessage;
    }

    void displayWinMessage()
    {
        finalText.text = string.Format(winText, 5);
        displayMessage();
    }

    void displayLostMessage()
    {
        finalText.text = lostText;
        displayMessage();
    }

	void displayMessage()
    {
        levelText.text = string.Format(level, 5);
        animator.SetTrigger("GameOver");
    }

    void hideMessage()
    {
        animator.SetTrigger("Restart");
    }
}
