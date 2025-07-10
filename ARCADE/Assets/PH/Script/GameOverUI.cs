// ---- IN�CIO DO C�DIGO CORRETO ----

using UnityEngine;
using UnityEngine.UI; // ESSA LINHA � CRUCIAL para o tipo "Button" funcionar!
using TMPro;          // Essa linha � para TMP_Text e TMP_InputField
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    // --- REFER�NCIAS DA UI ---
    // O Unity vai criar um campo no Inspector para cada uma dessas vari�veis p�blicas.
    public TMP_Text scoreText;
    public TMP_InputField nameInputField;
    public Button saveButton;

    private int finalScore;

    // Classe est�tica para passar a pontua��o entre as cenas
    public static class GameSession
    {
        public static int score;
    }

    void Start()
    {
        // Pega a pontua��o da sess�o de jogo e mostra na tela
        finalScore = GameSession.score;
        scoreText.text = "SUA PONTUA��O: " + finalScore.ToString();

        // IMPORTANTE: Garante que o bot�o n�o est� nulo antes de adicionar o listener
        if (saveButton != null)
        {
            // Adiciona um listener para o bot�o chamar a fun��o quando for clicado
            saveButton.onClick.AddListener(SaveScoreAndReturnToMenu);
        }
        else
        {
            Debug.LogError("O 'Save Button' n�o foi atribu�do no Inspector!");
        }
    }

    public void SaveScoreAndReturnToMenu()
    {
        // Pega o nome digitado pelo jogador
        string playerName = nameInputField.text;

        // Se o nome estiver em branco, usa um nome padr�o
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "JOGADOR";
        }

        // Adiciona a pontua��o usando o RankingManager (verifique se voc� tem esse script no projeto)
        if (RankingManager.Instance != null)
        {
            RankingManager.Instance.AddScore(playerName.ToUpper(), finalScore);
        }

        // Carrega a cena do menu principal
        SceneManager.LoadScene("MENU");
    }
}

// ---- FIM DO C�DIGO CORRETO ----