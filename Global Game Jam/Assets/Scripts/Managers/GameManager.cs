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

    int a=0;
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
        colorButtons[0] = new Color(1.0f, 0.0f, 0.0f); // Vermelho
        colorButtons[1] = new Color(0.0f, 0.0f, 1.0f); // Azul
        colorButtons[2] = new Color(1.0f, 0.92f, 0.016f); // Amarelo
        colorButtons[3] = new Color(0.0f, 1.0f, 0.0f); // Verde
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
        
        letters = new List<Letter>();
        

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
    
    private void Update() {
        if(Input.GetKeyDown(KeyCode.A)){
            VerificaCor(0);
            
        }
        if(Input.GetKeyDown(KeyCode.S)){
            VerificaCor(1);
            
        }
        if(Input.GetKeyDown(KeyCode.D)){
            VerificaCor(2);
            
        }
        if(Input.GetKeyDown(KeyCode.F)){
            VerificaCor(3);
            
        }
        UpdateWordText();
        if(Input.GetKeyDown(KeyCode.Space)){
            StartGame();
        }
    }

    public void VerificaCor(int index){
        Debug.Log(a);
        if (a < letters.Count){
            Letter letter = letters[a];
            
            if (letters[a].color == colorButtons[index])
            {
                letter.isCorrect = true;
            }
            else
            {
                letter.isCorrect = true;
                //aumentaDificuldade();
            }
            letters[a] = letter;
            a++;
            if (letters[a].letter == ' ')
            {
                a++;
            }
        }else{
            Debug.LogError("Index out of range");
            StartGame();
            a=0;
        }
        UpdateWordText();
    }

    public void aumentaDificuldade(){
        //Fazendo uma pausa de 0.5 segundos
        StartCoroutine(PausaPorUmSegundo());


        Debug.Log("Aumentando dificuldade");
    }
    IEnumerator PausaPorUmSegundo()
    {
        

        // Aguarda por 1 segundo
        yield return new WaitForSeconds(1.0f);

        

        // Adicione aqui o código que você deseja executar após a pausa de 1 segundo
    }
}

