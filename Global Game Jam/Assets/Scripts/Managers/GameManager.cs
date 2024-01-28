using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Importa o pacote TextMeshPro

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image[] dialogImages;

    // Referência para os botões de cor
    public Color[] colorButtons;
    System.Random random = new System.Random();
    //craindo struct para armazenar a letra da piada, a cor se ela foi acertada ou não
    public struct Letter
    {
        public char letter;
        public Color color;
        public bool isCorrect;
    }
    //criando lista de letras
    public List<Letter> letters;
    //lista de piadas

    int a = 0;
    public string[] jokes;
    // Referência para os textos de pontuação e palavra

    private bool paused = true;
    private string currentJoke;
    private int score;
    public TextMeshProUGUI wordText;


    void Start()
    {
        // Inicializa o jogo
        GameController.Instance.startGamePlay += OnStartGamePlay;
        GameController.Instance.endGamePlay += OnEndGamePlay;

    }

    private void OnDisable()
    {
        GameController.Instance.startGamePlay -= OnStartGamePlay;
        GameController.Instance.endGamePlay -= OnEndGamePlay;
    }

    private void OnEndGamePlay()
    {
        wordText.text = "";
        letters.Clear();
        for (int i = 0; i < dialogImages.Length; i++)
        {
            dialogImages[i].enabled = false;
        }



    }

    private void OnStartGamePlay()
    {
        StartGame();
        paused = false;
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
                                                       // jokes = new string[3];
                                                       //jokes[0] = "O que o pato disse para a pata? R: Vem Quá";
                                                       //jokes[1] = "O que o pato disse para a pata? R: Num Vem Quá";
                                                       //jokes[2] = "O que o pato disse para a pata? R: Será que Vem Quá"; 

        // jokes[0] = "O que o pato disse para a pata? R: Vem Quá";
        // jokes[1] = "O que o pato disse para a pata? R:Num Vem Quá";
        // jokes[2] = "O que o pato disse para a pata? R:Será que Vem Quá";

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
            Color cor = colorButtons[randomIndex];
            if (currentJoke[i] == '?' || currentJoke[i] == '.' || currentJoke[i] == '!' || currentJoke[i] == ',' || currentJoke[i] == ':' || currentJoke[i] == ';' || currentJoke[i] == '-')
                cor = Color.black;


            letters.Add(new Letter { letter = currentJoke[i], color = cor, isCorrect = false });
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
            if (!letters[i].isCorrect)
            {
                wordText.text += "<color=#" + ColorUtility.ToHtmlStringRGB(letters[i].color) + ">" + letters[i].letter + "</color>";
            }
            else
            {
                wordText.text += "<color=#000000>" + letters[i].letter + "</color>";
            }
        }
    }

    private void Update()
    {
        if (paused) return;
        if (Input.GetKeyDown(KeyCode.A))
        {
            VerificaCor(0);

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            VerificaCor(1);

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            VerificaCor(2);

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            VerificaCor(3);

        }
        UpdateWordText();
    }

    public void VerificaCor(int index)
    {
        // Debug.Log(a);
        if (a < letters.Count)
        {
            Letter letter = letters[a];

            if (letters[a].color == colorButtons[index])
            {
                letter.isCorrect = true;
                GameController.Instance.AddScore(1);
            }
            else
            {
                letter.isCorrect = true;
                aumentaDificuldade(-0.1f);
            }
            letters[a] = letter;
            a++;

            if (letters.Count <= a)
            {
                GameController.Instance.FinishAJoke();
                StartCoroutine(PausaDepoisPiada(1));
                GameController.Instance.AddScore(25);
            }
            else if (letters[a].letter == ' ' || letters[a].letter == '?' ||
                    letters[a].letter == '.' || letters[a].letter == '!' ||
                    letters[a].letter == ',' || letters[a].letter == ':' ||
                    letters[a].letter == ';' || letters[a].letter == '-')
            {
                a++;
            }

            UpdateWordText();

            if (letters.Count <= a)
            {
                
                GameController.Instance.FinishAJoke();
                StartCoroutine(PausaDepoisPiada(1));
                GameController.Instance.AddScore(25);
                
            }
        }

    }

    public void aumentaDificuldade(float amount)
    {
        //Fazendo uma pausa de 0.5 segundos
        StartCoroutine(PausaPorUmSegundo(1));
        GameController.Instance.ChangeDificulty(amount);

        //Debug.Log("Aumentando dificuldade");
    }
    IEnumerator PausaPorUmSegundo(float time)
    {
        paused = true;

        // Aguarda por 1 segundo
        yield return new WaitForSeconds(time);


        paused = false;
        // Adicione aqui o código que você deseja executar após a pausa de 1 segundo
    }

    IEnumerator PausaDepoisPiada(float time)
    {
        paused = true;
        GameController.Instance.StopFiring();

        aumentaDificuldade(1);
        a = 0;


        // Aguarda por 1 segundo
        yield return new WaitForSeconds(time);

        StartGame();
        GameController.Instance.StartFiring();
        paused = false;
        // Adicione aqui o código que você deseja executar após a pausa de 1 segundo
    }
}

