using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public Action<int> playerDamage;
    public Action startGame;
    public Action startGamePlay;
    public Action endGame;
    public Action endGamePlay;
    public Action<float> changeDificulty;
    public Action finishAJoke;
    public Action stopFiring;
    public Action startFiring;
    private int score;

    [Header("Intro Cutscene")]
    [SerializeField] private Image introBlackScreen;

    [Header("Audio")]
    [SerializeField] private AudioSource[] audioSource;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private String[] audioClipsNames;
    [SerializeField] private AudioSource mainMusic;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject finalScore;
    [SerializeField] private TMP_Text finalScoreText;


    public static GameController Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startGame += StartGameCutscene;
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        // if(Keyboard.current.spaceKey.wasPressedThisFrame)
        // {
        //     StartGamePlay();
        // }
    }

    public int GetScore()
    {
        return score;
    }

    public void AddScore(int score)
    {
        this.score += score;
        scoreText.text = "Score: " + this.score.ToString();
    }

    public void StartGameCutscene()
    {
        StartCoroutine(StartGameCutsceneCoroutine());
    }

    private IEnumerator StartGameCutsceneCoroutine()
    {
        do
        {
            introBlackScreen.color = new Color(0, 0, 0, introBlackScreen.color.a - 0.01f);
            yield return new WaitForSeconds(0.001f);
        } while (introBlackScreen.color.a > 0);

        yield return new WaitForSeconds(1f);
        

        while(!Keyboard.current.spaceKey.isPressed){
            yield return null;
        }
        
        StartGamePlay();
    }

    public void StartGamePlay()
    {
        startGamePlay?.Invoke();
    }

    public void EndGamePlay()
    {
        finalScore.SetActive(true);
        endGamePlay?.Invoke();
        finalScoreText.text = "Final Score:\n" + score.ToString() + "\n Press space to menu";

        EndGame();
    }

    public void DamagePlayer(int damage)
    {
        playerDamage(damage);
    }

    public void EndGame()
    {
        endGame?.Invoke();

        StartCoroutine(EndGameCoroutine());
    }

    private IEnumerator EndGameCoroutine()
    {

        while (!Keyboard.current.spaceKey.isPressed)
        {
            yield return null;
        }

        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        startGame?.Invoke();
    }

    public void ChangeDificulty(float amount)
    {
        changeDificulty?.Invoke(amount);
    }

    public void FinishAJoke()
    {
        finishAJoke?.Invoke();
    }

    public void StopFiring()
    {
        stopFiring?.Invoke();
    }

    public void StartFiring()
    {
        startFiring?.Invoke();
    }

    public void PlayAudio(String name)
    {
        var index = Array.IndexOf(audioClipsNames ,name);

        if(index != -1)
        {
            for(int i = 0; i < audioSource.Length; i++)
            {
                if(audioSource[i].isPlaying)
                    continue;
                
                audioSource[i].clip = audioClips[index];
                audioSource[i].Play();
                //Debug.Log(i);
                break;
            }
        }
    }
}
