using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCurrentLevel : MonoBehaviour {

    public List<Text> textList;

	// Use this for initialization
	void Start () {
		foreach(var text in textList)
        {
            text.text = GeneralManager.dificultyLevel.ToString();
        }
	}
}
