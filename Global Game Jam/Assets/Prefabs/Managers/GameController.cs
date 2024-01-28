using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro; 

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public Action<int> playerDamage;
    public Action startGame;
    public Action startGamePlay;
    public Action endGame;
    public Action endGamePlay;
    public Action changeDificulty;
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
            DontDestroyOnLoad(gameObject);
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
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartGamePlay();
        }
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
        yield return new WaitForSeconds(1f);
    }

    public void StartGamePlay()
    {
        startGamePlay?.Invoke();
    }

    public void EndGamePlay()
    {
        endGamePlay?.Invoke();
    }

    public void DamagePlayer(int damage)
    {
        playerDamage(damage);
    }

    public void EndGame()
    {
        endGame?.Invoke();
    }

    public void StartGame()
    {
        startGame?.Invoke();
    }

    public void ChangeDificulty()
    {
        changeDificulty?.Invoke();
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
