using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BackupManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameController.OnGameOver += reloadSceneDelayed;
        GameController.OnGameWon += reloadSceneDelayed;
    }

    void reloadSceneDelayed()
    {
        StartCoroutine(reloadScene());
    }

    IEnumerator reloadScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        GameController.OnGameOver -= reloadSceneDelayed;
        GameController.OnGameWon -= reloadSceneDelayed;
    }
}
