using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    public GameObject arrowJ1;
    public GameObject arrowJ2;
    public float discordAngle;

    public float arrowDistance = 1f;

    [Header("Death")]
    public GameObject rendering;
    public ParticleSystem deathByElecticity;
    bool dead = false;

    public enum PlayerState
    {
        toBomb,
        toStart
    };

    [Header("Config")]
    public float Speed;

    Rigidbody2D rb;

    Vector2 velocity;

    private Trace _trace;
    private PlayerState _playerState;

    public static System.Action eDied;

    private void Awake()
    {
        startPos = transform.position;
        rb = gameObject.GetComponent<Rigidbody2D>();
        _playerState = PlayerState.toBomb;

        DeadZone.OnPlayerEnterDeadZone += Die;
        Bomb.OnBombExplosion += Die;
    }

    private void OnDestroy()
    {
        DeadZone.OnPlayerEnterDeadZone -= Die;
        Bomb.OnBombExplosion -= Die;
    }

    public void Die()
    {
        if (!dead)
            StartCoroutine(DieByElectricity());
    }

    void Update()
    {
        Vector2 moveJ1;
        Vector2 moveJ2;
        moveJ1 = new Vector2(Input.GetAxis("HorizontalJ1"), Input.GetAxis("VerticalJ1"));
        moveJ2 = new Vector2(Input.GetAxis("HorizontalJ2"), Input.GetAxis("VerticalJ2"));
        discordAngle = Vector2.Angle(moveJ1, moveJ2);

        arrowJ1.transform.localPosition = (Vector3)moveJ1 * arrowDistance + Vector3.forward * 0.1f;
        arrowJ2.transform.localPosition = (Vector3)moveJ2 * arrowDistance + Vector3.forward * 0.1f;
        arrowJ1.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(moveJ1.x, moveJ1.y, 0));
        arrowJ2.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(moveJ2.x, moveJ2.y, 0));
        Vector2 move = moveJ1 + moveJ2;

        rb.velocity -= velocity;
        if (dead)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            velocity = move * Speed * Time.deltaTime;
        }
        rb.velocity += velocity;

        if (_playerState == PlayerState.toBomb)
            _trace.addPoint(transform.position);
    }

    public void setTrace(Trace trace)
    {
        _trace = trace;
    }

    
    public PlayerState getPlayerState()
    {
        return _playerState;
    }

    public void setPlayerState(PlayerState playerState)
    {
        _playerState = playerState;
    }

    Vector3 startPos;

    public IEnumerator DieByElectricity()
    {
        rendering.SetActive(false);
        deathByElecticity.Emit(100);
        dead = true;
        rb.simulated = false;
        //rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(2);
        if (eDied != null)
        {
            eDied();
        }

        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //rb.simulated = true;
        //rendering.SetActive(true);
        //transform.position = startPos;
        //dead = false;
        //rb.velocity = Vector3.zero;
    }
    
}
