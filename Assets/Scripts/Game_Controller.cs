using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState { Idle,Playing,Ended,Ready};
public class Game_Controller : MonoBehaviour
{
    [Range(0f,0.20f)]
    public float parallaxSpeed = 0.02f;
    public RawImage background;
    public RawImage platform;
    public GameObject uiIdle;
    public GameObject uiScore;
    public Text pointsText;
    public Text recordText;

    public GameState gameState = GameState.Idle;
    public GameObject player;

    public GameObject enemyGenerator;

    public float scaleTime = 6f;
    public float scaleInc = 0.25f;

    private AudioSource musicPlayer;
    private int points =0;
    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
        recordText.text = "BEST SCORE: " + GetMaxScore().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        bool userAction = Input.GetKeyDown("up") || Input.GetMouseButtonDown(0);
        //Empieza el juego
        if (gameState== GameState.Idle && userAction )
        {
            gameState = GameState.Playing;
            uiIdle.SetActive(false);
            uiScore.SetActive(true);
            player.SendMessage("UpdateState", "PlayerRun");
            enemyGenerator.SendMessage("StartGenerator");
            musicPlayer.Play();
            InvokeRepeating("GameTimeScale", scaleTime, scaleTime);//llamamos al timescale 6seg despues del juego en marcha y cada 6 seg que se repita
        }

        //Juego en marcha
        else if(gameState ==GameState.Playing)
        {
            Parallax();
        }
        //Juego listo para reiniciarse
        else if(gameState == GameState.Ready)
        {
            if(userAction) //Si el jugador aprieta click o la flecha hacia arriba se resetea el juego
            {
                RestartGame();
            }
        }
       
    }

    public void Parallax()
    {
        float finalSpeed = parallaxSpeed * Time.deltaTime;
        background.uvRect = new Rect(background.uvRect.x + finalSpeed, 0f, 1f, 1f);
        platform.uvRect = new Rect(platform.uvRect.x + finalSpeed * 4, 0f, 1f, 1f);
    }

    public void RestartGame() //Metodo para resetear el juego despues de muerte
    {
        SceneManager.LoadScene("SampleScene");
    }

    void GameTimeScale()
    {
        Time.timeScale += scaleInc;
        Debug.Log("Ritmo incrementado " + Time.timeScale.ToString());
    }

    public void ResetTimeScale()
    {
        CancelInvoke("GameTimeScale");
        Time.timeScale = 1f;
        Debug.Log("Ritmo restablecido");
    }

    public void IncreasePoints()
    {
        pointsText.text ="Score "+(++points).ToString();

        if (points >= GetMaxScore())
        {
            recordText.text = "BEST SCORE: " + points.ToString();
            SaveScore(points);
        }
    }

    public int GetMaxScore()
    {
        return PlayerPrefs.GetInt("Max Points", 0);
    }

    public void SaveScore(int currentPoints)
    {
        PlayerPrefs.SetInt("Max Points", currentPoints);
    }
}
