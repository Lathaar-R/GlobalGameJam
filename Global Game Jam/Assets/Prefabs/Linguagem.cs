using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Linguagem : MonoBehaviour
{

    public bool portugues;
    public Image check;


    private void Awake()
    {
        var obj = GameObject.Find("Linguagem");
        if(obj != null && obj != gameObject)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        var c = GameObject.Find("Check");

        if (c != null)
        {
            check = c.GetComponent<Image>();
            check.enabled = portugues;
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        var c = GameObject.Find("Check");

        if (c != null)
        {
            check = c.GetComponent<Image>();
            portugues = false;
            var button = GameObject.Find("LButton");
            var b = button.GetComponent<Button>();
            b.onClick.AddListener(SetPortugues);
        }
    }

    public void SetPortugues()
    {
        portugues = !portugues;
        Debug.Log("Portugues: " + portugues);

        if (check != null)
        {
            check.enabled = portugues;
        }
    }



}
