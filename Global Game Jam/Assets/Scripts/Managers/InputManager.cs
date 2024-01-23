using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputMenager : MonoBehaviour
{
    // instancia do singleton
    public static InputMenager Instance { get; private set; }
    public GameInputs Inputs { get; private set; }
    void Awake()
    {
        //instanciando characterController
        Inputs = new GameInputs();
        Inputs.Enable();

        //cria a instancia do singleton
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}