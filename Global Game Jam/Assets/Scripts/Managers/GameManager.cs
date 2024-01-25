using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   
using TMPro; // Importa o pacote TextMeshPro

public class GameManager : MonoBehaviour
{
    // Referência para os botões de cor
    public Color[] colorButtons;
    System.Random random = new System.Random();
    //craindo struct para armazenar a letra da piada, a cor se ela foi acertada ou não
    public struct Letter{
        public char letter;
        public Color color;
        public bool isCorrect;
    }
    //criando lista de letras
    public List<Letter> letters;
    //lista de piadas
    public string[] jokes;
    // Referência para os textos de pontuação e palavra

    private string currentJoke;
    private int score;
    public TextMeshProUGUI wordText;

    void Start()
    {
        
        
        // Inicializa o jogo
        StartGame();
    }

    void StartGame()
    {
        //inicializando as cores dos botões
        colorButtons = new Color[4];
        colorButtons[0] = new Color(1.0f, 0.0f, 0.0f);
        colorButtons[1] = new Color(0.0f, 0.0f, 1.0f);
        colorButtons[2] = new Color(1.0f, 0.92f, 0.016f);
        colorButtons[3] = new Color(0.0f, 1.0f, 0.0f);
        //inicializando a lista de letras
        //letters = new List<Letter>();
        //inicializando a lista de piadas
        jokes = new string[3];
        //jokes[0] = "O que o pato disse para a pata? R: Vem Quá";
        //jokes[1] = "O que o pato disse para a pata? R: Num Vem Quá";
        //jokes[2] = "O que o pato disse para a pata? R: Será que Vem Quá"; 
        
        jokes[0] = "O que o pato disse para a pata? R: Vem Quá";
        jokes[1] = "O que o pato disse para a pata? R:Num Vem Quá";
        jokes[2] = "O que o pato disse para a pata? R:Será que Vem Quá";

        // Escolhe uma palavra aleatória
        currentJoke = GetRandomJoke();
        
        // Pinta cada letra da piada de uma cor aleatória
            
        if (colorButtons == null || colorButtons.Length == 0)
        {
            Debug.LogError("colorButtons is not initialized or is empty");
            return;
        }

        // Ensure letters is initialized
        if (letters == null)
        {
            letters = new List<Letter>();
        }

        Debug.Log(currentJoke);




        
        for (int i = 0; i < currentJoke.Length; i++)
        {
            int randomIndex = random.Next(0, colorButtons.Length);
            letters.Add(new Letter{letter = currentJoke[i], color = colorButtons[randomIndex], isCorrect = false});
        }
        //wordText.text = currentJoke;

        // Atualiza o texto da palavra
        UpdateWordText();

        
    }

    string GetRandomJoke()
    {
        //gerando um numero aleatorio ate o tamanho da lista de piadas
        int randomIndex = random.Next(jokes.Length);
    
        //retornando a piada na posição randomIndex
        return jokes[randomIndex];
        
    }

    public void UpdateWordText()
    {
        // Atualiza o texto da palavra
        wordText.text = "";
        for (int i = 0; i < letters.Count; i++)
        {
            if(!letters[i].isCorrect){
                wordText.text += "<color=#" + ColorUtility.ToHtmlStringRGB(letters[i].color) + ">" + letters[i].letter + "</color>";
            }else{
                wordText.text += "<color=#000000>" + letters[i].letter + "</color>";
            }
        }
    }   
}

