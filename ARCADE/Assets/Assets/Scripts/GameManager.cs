using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Define os possíveis estados do turno, incluindo GameOver.
public enum TurnState { PlayerTurn, EnemyTurn, GameOver }

public class GameManager : MonoBehaviour
{
    // --- PADRÃO SINGLETON ---
    public static GameManager Instance;

    // --- SEÇÃO DE UI ---
    [Header("UI do Jogo")]
    public TextMeshProUGUI textoScore;
    private int score;

    // --- SEÇÃO DE CONTROLE DE TURNOS ---
    [Header("Controle de Turnos")]
    public TurnState CurrentState { get; private set; }
    public List<EnemyController> enemies;
    private int enemyIndex = 0;

    // --- SEÇÃO DE CONFIGURAÇÃO DE JOGO (Mantida para referência) ---
    [Header("Configuração de Jogo")]
    public GameObject bolaInicialPrefab;
    public Transform pontoDeSpawn;

    // O método Awake configura o Singleton.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // O método Start inicializa o estado do jogo.
    void Start()
    {
        // Garante que o tempo volte ao normal quando a cena do jogo carrega ou reinicia.
        Time.timeScale = 1f;

        score = 0;
        AtualizarTextoDoScore();
        StartPlayerTurn();
    }

    // --- MÉTODOS PÚBLICOS PARA GERENCIAR PONTUAÇÃO ---
    public void AdicionarPontos(int pontosParaAdicionar)
    {
        if (CurrentState == TurnState.GameOver) return;
        score += pontosParaAdicionar;
        AtualizarTextoDoScore();
    }

    // --- MÉTODOS PARA GERENCIAR TURNOS ---
    public void StartPlayerTurn()
    {
        if (CurrentState == TurnState.GameOver) return;
        CurrentState = TurnState.PlayerTurn;
    }

    public void EndPlayerTurn()
    {
        if (CurrentState == TurnState.GameOver) return;
        CurrentState = TurnState.EnemyTurn;
        enemyIndex = 0;
        StartCoroutine(ProcessEnemies());
    }

    IEnumerator ProcessEnemies()
    {
        while (enemyIndex < enemies.Count)
        {
            if (CurrentState == TurnState.GameOver) yield break;
            if (enemies[enemyIndex] != null)
            {
                enemies[enemyIndex].PerformAction(FindAnyObjectByType<PlayerController>());
            }
            enemyIndex++;
            yield return new WaitForSeconds(0.5f);
        }
        StartPlayerTurn();
    }

    public void EndEnemyTurn() { }

    // --- MÉTODO ATUALIZADO PARA INICIAR A SEQUÊNCIA DE GAME OVER ---
    // --- MÉTODO ATUALIZADO PARA INICIAR A SEQUÊNCIA DE GAME OVER ---
    public void StartGameOver()
    {
        // Se o jogo já acabou, não faz nada para evitar chamadas múltiplas.
        if (CurrentState == TurnState.GameOver) return;

        CurrentState = TurnState.GameOver;
        Debug.Log("GAME OVER INICIADO! Pontuação final: " + score);

        // --- MUDANÇA PRINCIPAL AQUI ---
        // Em vez de usar uma classe estática, salvamos a pontuação diretamente
        // nos PlayerPrefs com uma chave temporária.
        PlayerPrefs.SetInt("TempFinalScore", score);
        PlayerPrefs.Save(); // Força a gravação dos dados no disco imediatamente.
                            // ---------------------------------

        // 1. Congela o tempo do jogo.
        Time.timeScale = 0f;

        // 2. Chama o FadeManager para cuidar da transição de cena de forma suave.
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeParaCena("GAMEOVER");
        }
        else
        {
            // Plano B: Se o FadeManager não for encontrado, carrega a cena diretamente.
            Debug.LogError("FadeManager não encontrado! Carregando a cena de GameOver diretamente.");
            SceneManager.LoadScene("GAMEOVER");
        }
    }

    // --- MÉTODO AUXILIAR ---
    private void AtualizarTextoDoScore()
    {
        if (textoScore != null)
        {
            textoScore.text = "SCORE: " + score;
        }
        else
        {
            Debug.LogWarning("GameManager: A referência para o 'textoScore' não foi configurada no Inspector!");
        }
    }
}