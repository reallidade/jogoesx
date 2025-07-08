using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Necess�rio para controlar o texto da UI (TextMeshPro)

// Define os poss�veis estados do turno
public enum TurnState { PlayerTurn, EnemyTurn }

public class GameManager : MonoBehaviour
{
    // --- PADR�O SINGLETON ---
    // Uma refer�ncia est�tica e p�blica que permite que qualquer script acesse este
    // GameManager de forma f�cil e segura usando "GameManager.Instance".
    public static GameManager Instance;

    // --- SE��O DE UI ---
    [Header("UI do Jogo")]
    public TextMeshProUGUI textoScore; // Arraste seu objeto de texto do score aqui pelo Inspector!
    private int score;

    // --- SE��O DE CONTROLE DE TURNOS ---
    [Header("Controle de Turnos")]
    public TurnState CurrentState { get; private set; }
    public List<EnemyController> enemies;
    private int enemyIndex = 0;

    // --- SE��O DE CONFIGURA��O DE JOGO ---
    // A cria��o da bola inicial foi removida daqui, pois agora � responsabilidade
    // do seu BallSpawnerManager. Mantive as vari�veis caso precise delas para outra coisa.
    [Header("Configura��o de Jogo")]
    public GameObject bolaInicialPrefab;
    public Transform pontoDeSpawn;

    // O m�todo Awake � chamado antes de qualquer m�todo Start no projeto.
    // � o local ideal para configurar o Singleton.
    private void Awake()
    {
        // L�gica do Singleton: garante que exista apenas UMA inst�ncia do GameManager.
        if (Instance == null)
        {
            // Se n�o houver nenhuma inst�ncia, esta se torna a inst�ncia.
            Instance = this;
            // Opcional: Se seu jogo tiver m�ltiplas cenas, descomente a linha abaixo
            // para que o GameManager n�o seja destru�do ao carregar uma nova cena.
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Se uma inst�ncia j� existe, destr�i este objeto para evitar duplicatas.
            Destroy(gameObject);
        }
    }

    // O m�todo Start � chamado no primeiro frame em que o script est� ativo.
    void Start()
    {
        // Inicializa o score do jogador
        score = 0;
        AtualizarTextoDoScore();

        // Inicia o turno do jogador
        StartPlayerTurn();
    }

    // --- M�TODOS P�BLICOS PARA GERENCIAR PONTUA��O ---
    // Qualquer outro script (como o BolaController) chamar� este m�todo para dar pontos.
    public void AdicionarPontos(int pontosParaAdicionar)
    {
        score += pontosParaAdicionar;
        AtualizarTextoDoScore();
    }

    // M�todo privado para atualizar o texto na tela.
    private void AtualizarTextoDoScore()
    {
        // Verifica se a refer�ncia ao texto n�o � nula antes de us�-la.
        if (textoScore != null)
        {
            textoScore.text = "SCORE: " + score;
        }
        else
        {
            // Um aviso �til caso voc� esque�a de arrastar o texto no Inspector.
            Debug.LogWarning("GameManager: A refer�ncia para o 'textoScore' n�o foi configurada no Inspector!");
        }
    }

    // --- M�TODOS PARA GERENCIAR TURNOS (Sua l�gica original) ---
    public void StartPlayerTurn()
    {
        CurrentState = TurnState.PlayerTurn;
    }

    public void EndPlayerTurn()
    {
        CurrentState = TurnState.EnemyTurn;
        enemyIndex = 0;
        StartCoroutine(ProcessEnemies());
    }

    IEnumerator ProcessEnemies()
    {
        while (enemyIndex < enemies.Count)
        {
            // Garante que o inimigo ainda existe antes de tentar agir.
            if (enemies[enemyIndex] != null)
            {
                enemies[enemyIndex].PerformAction(FindObjectOfType<PlayerController>());
            }
            enemyIndex++;
            yield return new WaitForSeconds(0.5f);
        }
        StartPlayerTurn();
    }

    public void EndEnemyTurn()
    {
        // A l�gica � controlada pela corrotina ProcessEnemies.
    }
}