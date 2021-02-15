using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState { Idle, Playing, Ended, Ready };

public class GameController : MonoBehaviour
{
    [Range(0f, 0.20f)]
    public float parallaxSpeed = 0.02f;
    public RawImage fondo;
    public RawImage plataforma;
    public GameObject uiIdle;
    public GameObject uiScore;
    public GameObject player;
    public GameObject enemyGenerator;

    private AudioSource musicplayer;

    public float scaleTime = 6f;
    public float scaleInc = .25f;

    private int points = 0;
    public Text pointsText;
    public Text recordText;


    

    public GameState gameState = GameState.Idle;

    // Start is called before the first frame update
    void Start()
    {
        musicplayer = GetComponent<AudioSource>();
        recordText.text = "BEST: " + GetMaxScore().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Empieza el juego
        if (gameState == GameState.Idle && (Input.GetKeyDown("up") || Input.GetMouseButtonDown(0)))
        {
            gameState = GameState.Playing;
            uiIdle.SetActive(false);
            uiScore.SetActive(true);
            player.SendMessage("UpdateState", "StartRunning");
            enemyGenerator.SendMessage("StartGeneration");
            musicplayer.Play();
            InvokeRepeating("GameTimeScale", scaleTime, scaleTime);
        }

        else if (gameState == GameState.Playing)
        {
            Parallax();
        }

        else if (gameState == GameState.Ready)
        {
            if (Input.GetKeyDown("up") || Input.GetMouseButtonDown(0))
            {
                RestartGame();
            }
        }
    }
        void Parallax()
        {
            float finalSpeed = parallaxSpeed * Time.deltaTime;
            fondo.uvRect = new Rect(fondo.uvRect.x + finalSpeed, 0f, 1f, 1f);
            plataforma.uvRect = new Rect(plataforma.uvRect.x + finalSpeed * 4, 0f, 1f, 1f);
        }

        void RestartGame()
        {
            RestTimeScale();
            SceneManager.LoadScene("SampleScene");
        }

        void GameTimeScale()
        {
            Time.timeScale += scaleInc;
            Debug.Log("Ritmo Incrementado: " + Time.timeScale.ToString());
        }

        public void RestTimeScale(float newTimeScale = 1f)
        {
            CancelInvoke("GameTimeScale");
            Time.timeScale = newTimeScale;
            Debug.Log("Ritmo Restablecido: " + Time.timeScale.ToString());
        }

    public void IncreasePoints()
    {
        points++;
        pointsText.text = points.ToString();
        if (points >= GetMaxScore())
        {
            recordText.text = "BEST: " + points.ToString();
            SaveScore(points);
        }
    }

    public int GetMaxScore()
    {
        return PlayerPrefs.GetInt("Max Points", 0);
    }

    public void SaveScore(int currentsPoints)
    {
        PlayerPrefs.SetInt("Max Points", currentsPoints);
    }

        
    
}
