using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Bomb : MonoBehaviour {

    public delegate void BombEvent();
    public static event BombEvent OnPlayerInitiateBomb;
    public static event BombEvent OnBombExplosion;

    public GameObject explosion;


    bool bombInitiated = false;
    static bool bombExplosed = false;

    private void Awake()
    {
        OnPlayerInitiateBomb += initiateBomb;
        OnBombExplosion += explode;
        bombExplosed = false;
    }

    public void OnDestroy()
    {
        OnBombExplosion -= explode;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !bombInitiated)
        {
            Debug.Log("OnPlayerInitiateBomb");
            //collision.gameObject.GetComponent<PlayerController>().setPlayerState(PlayerController.PlayerState.toStart);
            OnPlayerInitiateBomb();
        }
    }

    void initiateBomb()
    {
        bombInitiated = true;
    }

    public static void TriggerBomb()
    {
        if (OnBombExplosion != null && !bombExplosed)
        {
            OnBombExplosion();
            bombExplosed = true;        
        }
    }

    public void explode()
    {
        Destroy(gameObject);
        SceneManager.SetActiveScene(gameObject.scene);
        GameObject go = Instantiate(explosion);
        go.transform.position = transform.position;

    }
}
