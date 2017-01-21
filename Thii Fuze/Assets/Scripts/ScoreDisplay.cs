using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour 
{
	public Text score1;
	public Text score2;
	public Text score3;
	public Text score4;
	public Text score5;
	public Text score6;
	public Text score7;
	public Text score8;
	public Text score9;
	public Text score10;

	void OnEnable () 
	{
		score1.text = "1 : " + HighScore.hs.highScores[0];
		score2.text = "2 : " + HighScore.hs.highScores[1];
		score3.text = "3 : " + HighScore.hs.highScores[2];
		score4.text = "4 : " + HighScore.hs.highScores[3];
		score5.text = "5 : " + HighScore.hs.highScores[4];
		score6.text = "6 : " + HighScore.hs.highScores[5];
		score7.text = "7 : " + HighScore.hs.highScores[6];
		score8.text = "8 : " + HighScore.hs.highScores[7];
		score9.text = "9 : " + HighScore.hs.highScores[8];
		score10.text = "10 : " + HighScore.hs.highScores[9];

	}
}
