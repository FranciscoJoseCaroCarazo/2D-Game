using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Animator animator;
    private AudioSource audioPlayer;
    public AudioClip jumpClip;
    public AudioClip dieClip;
    public AudioClip pointClip;


    public GameObject game;
    public GameObject enemyGeneration;
    private float startY;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        bool isGrounded = transform.position.y == startY;
        bool gamePlaying = game.GetComponent<GameController>().gameState == GameState.Playing;
        if (isGrounded && gamePlaying && (Input.GetKeyDown("up") || Input.GetMouseButtonDown(0)))
        {
            UpdateState("JumpAnimation");
            audioPlayer.clip = jumpClip;
            audioPlayer.Play();
        }
    }

    public void UpdateState(string state = null)
    {
        if (state != null)
        {
            animator.Play(state);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            UpdateState("playerDead");
            game.GetComponent<GameController>().gameState = GameState.Ended;
            enemyGeneration.SendMessage("StopGeneration", true);
            game.SendMessage("RestTimeScale", 0.5f);

            game.GetComponent<AudioSource>().Stop();
            audioPlayer.clip = dieClip;
            audioPlayer.Play();
        }
        else if (other.gameObject.tag == "point")
        {
            game.SendMessage("IncreasePoints");
            audioPlayer.clip = pointClip;
            audioPlayer.Play();
        }

    }

    void gameReady()
    {
        game.GetComponent<GameController>().gameState = GameState.Ready;
    }

}
