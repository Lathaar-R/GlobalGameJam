using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update

    public void TrocarCena()
    {
        // Carrega a cena com o nome fornecido
        SceneManager.LoadScene("Tutorial");
    }

    public void Sair()
    {
        // Sai do jogo
        Application.Quit();
    }

    public void Jogar()
    {
        SceneManager.LoadScene("Rodrigo");
    }
}
