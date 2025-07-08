using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public enum TurnState { PlayerTurn, EnemyTurn, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI do Jogo")]
    public TextMeshProUGUI textoScore;
    private int score;

    [Header("Game Over")]
    public Animator fadeAnimator;

    [Header("Controle de Turnos")]
    public TurnState CurrentState { get; private set; }
    public List<EnemyController> enemies;
    private int enemyIndex = 0;

    [Header("Configuração de Jogo")]
    public GameObject bolaInicialPrefab;
    public Transform pontoDeSpawn;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        // IMPORTANTE: Garante que o tempo volte ao normal quando a cena do jogo carrega.
        Time.timeScale = 1f;

        score = 0;
        AtualizarTextoDoScore();
        StartPlayerTurn();
    }

    public void AdicionarPontos(int pontosParaAdicionar)
    {
        if (CurrentState == TurnState.GameOver) return;
        score += pontosParaAdicionar;
        AtualizarTextoDoScore();
    }

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

    public void StartGameOver()
    {
        if (CurrentState == TurnState.GameOver) return;
        CurrentState = TurnState.GameOver;
        Debug.Log("GAME OVER INICIADO!");

        // NOVO: CONGELA O JOGO IMEDIATAMENTE!
        Time.timeScale = 0f;

        if (fadeAnimator != null)
        {
            fadeAnimator.gameObject.SetActive(true);
            fadeAnimator.SetTrigger("StartFade");
        }
        else
        {
            Debug.LogError("GameManager: O 'fadeAnimator' não foi configurado no Inspector!");
            SceneManager.LoadScene("GAMEOVER");
        }
    }

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