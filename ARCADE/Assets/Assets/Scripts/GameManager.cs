using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Necessário para controlar o texto da UI (TextMeshPro)

// Define os possíveis estados do turno
public enum TurnState { PlayerTurn, EnemyTurn }

public class GameManager : MonoBehaviour
{
    // --- PADRÃO SINGLETON ---
    // Uma referência estática e pública que permite que qualquer script acesse este
    // GameManager de forma fácil e segura usando "GameManager.Instance".
    public static GameManager Instance;

    // --- SEÇÃO DE UI ---
    [Header("UI do Jogo")]
    public TextMeshProUGUI textoScore; // Arraste seu objeto de texto do score aqui pelo Inspector!
    private int score;

    // --- SEÇÃO DE CONTROLE DE TURNOS ---
    [Header("Controle de Turnos")]
    public TurnState CurrentState { get; private set; }
    public List<EnemyController> enemies;
    private int enemyIndex = 0;

    // --- SEÇÃO DE CONFIGURAÇÃO DE JOGO ---
    // A criação da bola inicial foi removida daqui, pois agora é responsabilidade
    // do seu BallSpawnerManager. Mantive as variáveis caso precise delas para outra coisa.
    [Header("Configuração de Jogo")]
    public GameObject bolaInicialPrefab;
    public Transform pontoDeSpawn;

    // O método Awake é chamado antes de qualquer método Start no projeto.
    // É o local ideal para configurar o Singleton.
    private void Awake()
    {
        // Lógica do Singleton: garante que exista apenas UMA instância do GameManager.
        if (Instance == null)
        {
            // Se não houver nenhuma instância, esta se torna a instância.
            Instance = this;
            // Opcional: Se seu jogo tiver múltiplas cenas, descomente a linha abaixo
            // para que o GameManager não seja destruído ao carregar uma nova cena.
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Se uma instância já existe, destrói este objeto para evitar duplicatas.
            Destroy(gameObject);
        }
    }

    // O método Start é chamado no primeiro frame em que o script está ativo.
    void Start()
    {
        // Inicializa o score do jogador
        score = 0;
        AtualizarTextoDoScore();

        // Inicia o turno do jogador
        StartPlayerTurn();
    }

    // --- MÉTODOS PÚBLICOS PARA GERENCIAR PONTUAÇÃO ---
    // Qualquer outro script (como o BolaController) chamará este método para dar pontos.
    public void AdicionarPontos(int pontosParaAdicionar)
    {
        score += pontosParaAdicionar;
        AtualizarTextoDoScore();
    }

    // Método privado para atualizar o texto na tela.
    private void AtualizarTextoDoScore()
    {
        // Verifica se a referência ao texto não é nula antes de usá-la.
        if (textoScore != null)
        {
            textoScore.text = "SCORE: " + score;
        }
        else
        {
            // Um aviso útil caso você esqueça de arrastar o texto no Inspector.
            Debug.LogWarning("GameManager: A referência para o 'textoScore' não foi configurada no Inspector!");
        }
    }

    // --- MÉTODOS PARA GERENCIAR TURNOS (Sua lógica original) ---
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
        // A lógica é controlada pela corrotina ProcessEnemies.
    }
}