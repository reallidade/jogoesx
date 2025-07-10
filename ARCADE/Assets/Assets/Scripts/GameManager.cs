using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Define os poss�veis estados do turno, incluindo GameOver.
public enum TurnState { PlayerTurn, EnemyTurn, GameOver }

public class GameManager : MonoBehaviour
{
    // --- PADR�O SINGLETON ---
    public static GameManager Instance;

    // --- SE��O DE UI ---
    [Header("UI do Jogo")]
    public TextMeshProUGUI textoScore;
    private int score;

    // --- SE��O DE CONTROLE DE TURNOS ---
    [Header("Controle de Turnos")]
    public TurnState CurrentState { get; private set; }
    public List<EnemyController> enemies;
    private int enemyIndex = 0;

    // --- SE��O DE CONFIGURA��O DE JOGO (Mantida para refer�ncia) ---
    [Header("Configura��o de Jogo")]
    public GameObject bolaInicialPrefab;
    public Transform pontoDeSpawn;

    // O m�todo Awake configura o Singleton.
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

    // O m�todo Start inicializa o estado do jogo.
    void Start()
    {
        // Garante que o tempo volte ao normal quando a cena do jogo carrega ou reinicia.
        Time.timeScale = 1f;

        score = 0;
        AtualizarTextoDoScore();
        StartPlayerTurn();
    }

    // --- M�TODOS P�BLICOS PARA GERENCIAR PONTUA��O ---
    public void AdicionarPontos(int pontosParaAdicionar)
    {
        if (CurrentState == TurnState.GameOver) return;
        score += pontosParaAdicionar;
        AtualizarTextoDoScore();
    }

    // --- M�TODOS PARA GERENCIAR TURNOS ---
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
                enemies[enemyIndex].PerformAction(FindObjectOfType<PlayerController>());
            }
            enemyIndex++;
            yield return new WaitForSeconds(0.5f);
        }
        StartPlayerTurn();
    }

    public void EndEnemyTurn() { }

    // --- M�TODO ATUALIZADO PARA INICIAR A SEQU�NCIA DE GAME OVER ---
    public void StartGameOver()
    {
        // Se o jogo j� acabou, n�o faz nada para evitar chamadas m�ltiplas.
        if (CurrentState == TurnState.GameOver) return;

        CurrentState = TurnState.GameOver;
        Debug.Log("GAME OVER INICIADO!");

        // 1. Congela o tempo do jogo.
        Time.timeScale = 0f;

        // 2. Chama o FadeManager para cuidar da transi��o de cena de forma suave.
        //    Certifique-se de que a cena "GAMEOVER 1" est� adicionada ao Build Settings.
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeParaCena("GAMEOVER 1");
        }
        else
        {
            // Plano B: Se o FadeManager n�o for encontrado, carrega a cena diretamente.
            Debug.LogError("FadeManager n�o encontrado! Carregando a cena de GameOver diretamente.");
            SceneManager.LoadScene("GAMEOVER 1");
        }
    }

    // --- M�TODO AUXILIAR ---
    private void AtualizarTextoDoScore()
    {
        if (textoScore != null)
        {
            textoScore.text = "SCORE: " + score;
        }
        else
        {
            Debug.LogWarning("GameManager: A refer�ncia para o 'textoScore' n�o foi configurada no Inspector!");
        }
    }
}