using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject game;
    public GameObject enemyGenerator;
    public AudioClip jumpClip;
    public AudioClip dieClip;
    public AudioClip pointClip;

    private Animator animator;
    private AudioSource audioPlayer;
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
        bool gamePlaying = game.GetComponent<Game_Controller>().gameState == GameState.Playing;//game playing es true si el juego esta en marcha y el jugador no ha muerto
        bool isGrounded = transform.position.y == startY; //el juegador esta en el suelo (se obtiene la posicion del jugador actual y se la compara con la de inicio)
        bool userAction = (Input.GetKeyDown("up") || Input.GetMouseButtonDown(0));//activo la tecla para saltar
        if (isGrounded && gamePlaying && userAction)
        {
            UpdateState("PlayerJump");
            
            audioPlayer.clip = jumpClip;//musica de salto
            audioPlayer.Play();//reproducir musica salto una sola vez
        }
    }

    public void UpdateState(string state = null)
    {
        if (state != null)
        {
            animator.Play(state);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")//Cuando el enemigo choca el  trigger se elimina para no estar sobrecargando la memoria con enemigos que ya salieron del plano
        {
            UpdateState("PlayerDie");
            game.GetComponent<Game_Controller>().gameState = GameState.Ended;
            enemyGenerator.SendMessage("CancelGenerator", true);
            game.SendMessage("ResetTimeScale");

            //Audios
            game.GetComponent<AudioSource>().Stop();//detenemos la musica main
            audioPlayer.clip = dieClip;//iniciamos musica de muerte
            audioPlayer.Play();//reproducir musica muerte una sola vez
        }
        else if(other.gameObject.tag == "Point")
        {
            game.SendMessage("IncreasePoints");

            audioPlayer.clip = pointClip;//iniciamos musica de punto
            audioPlayer.Play();//reproducir musica punto una sola vez
        }

    }

    void GameReady()
    {
        game.GetComponent<Game_Controller>().gameState = GameState.Ready;
    }
}
