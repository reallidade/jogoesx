using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))] // Garante que a bola sempre terá um Rigidbody2D
public class BolaController : MonoBehaviour
{
    // --- VARIÁVEIS PÚBLICAS (Ajustáveis no Inspector) ---
    [Header("Atributos da Bola")]
    public int vida;
    public GameObject bolaMenorPrefab; // Arraste o prefab da bola menor aqui (se houver)
    public TextMeshPro textoVida;

    [Header("Configurações de Spawn")]
    public float forcaImpulsoInicial = 3f; // Força do impulso ao sair do spawner (pode ajustar)

    // --- VARIÁVEIS PRIVADAS ---
    private int vidaInicialDestaBola;
    private Rigidbody2D rb;

    // Awake é chamado quando o objeto é criado. Ideal para pegar componentes.
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start é chamado no primeiro frame que o objeto está ativo.
    void Start()
    {
        // Guarda a vida inicial para calcular a vida das bolas menores na divisão.
        vidaInicialDestaBola = vida;
        AtualizarTextoVida();
    }

    // --- MÉTODO 1: CHAMADO PELO 'BallSpawnerManager' ---
    // Prepara uma bola que acabou de ser criada nas bordas da tela.
    public void InicializarParaSpawner(Vector2 direcao)
    {
        // 1. Coloca a bola na layer "SpawningBall" para atravessar a parede.
        gameObject.layer = LayerMask.NameToLayer("SpawningBall");

        // 2. Aplica o impulso para "chutar" a bola para dentro do cenário.
        rb.AddForce(direcao * forcaImpulsoInicial, ForceMode2D.Impulse);

        // 3. Inicia a rotina que vai reativar as colisões normais após um tempo.
        StartCoroutine(AtivarColisoesFinais());
    }

    // --- MÉTODO 2: CHAMADO QUANDO UMA BOLA MAIOR 'MORRE' E SE DIVIDE ---
    // Prepara uma bola que nasceu da divisão de outra.
    public void InicializarParaDivisao(int vidaDaBolaNova, Vector2 direcaoDoPulo)
    {
        // 1. Define a vida da nova bola.
        this.vida = vidaDaBolaNova;
        this.vidaInicialDestaBola = vidaDaBolaNova; // Importante atualizar a vida inicial também.
        AtualizarTextoVida();

        // 2. Aplica um "pulo" controlado para que as bolas se separem.
        // Usar 'velocity' dá um controle mais preciso que 'AddForce' para este caso.
        rb.linearVelocity = direcaoDoPulo;
    }

    // --- CORROTINA PARA ATIVAR A LAYER FINAL ---
    // Espera um pouco e depois coloca a bola na sua layer definitiva ("Bolas").
    IEnumerator AtivarColisoesFinais()
    {
        // Espera tempo suficiente para a bola sair de dentro da parede.
        // 0.5s é um bom valor inicial para evitar o "teleporte".
        yield return new WaitForSeconds(1.2f);

        // **A CORREÇÃO PRINCIPAL ESTÁ AQUI:**
        // Muda a layer para "Bolas". Agora ela colidirá com as paredes,
        // mas não com outras bolas (conforme a Matriz de Colisão).
        gameObject.layer = LayerMask.NameToLayer("Bolas");
    }

    // --- LÓGICA DE DANO E MORTE ---
    public void LevarDano(int dano)
    {
        vida -= dano;
        if (vida <= 0)
        {
            Morrer();
        }
        else
        {
            AtualizarTextoVida();
        }
    }

    private void Morrer()
    {
        // Só tenta se dividir se houver um prefab de bola menor definido.
        if (bolaMenorPrefab != null)
        {
            int vidaDasBolasMenores = vidaInicialDestaBola / 2;
            if (vidaDasBolasMenores < 1) vidaDasBolasMenores = 1;

            // Bola 1 (Esquerda)
            GameObject bola1_objeto = Instantiate(bolaMenorPrefab, transform.position, Quaternion.identity);
            bola1_objeto.GetComponent<BolaController>().InicializarParaDivisao(vidaDasBolasMenores, new Vector2(-2f, 3f));

            // Bola 2 (Direita)
            GameObject bola2_objeto = Instantiate(bolaMenorPrefab, transform.position, Quaternion.identity);
            bola2_objeto.GetComponent<BolaController>().InicializarParaDivisao(vidaDasBolasMenores, new Vector2(2f, 3f));
        }

        // Destrói a bola original.
        Destroy(gameObject);
    }

    // --- MÉTODOS AUXILIARES ---
    private void AtualizarTextoVida()
    {
        if (textoVida != null)
        {
            textoVida.text = vida.ToString();
        }
    }

    // Detecta a colisão com o projétil do jogador.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Certifique-se que seu projétil tem a Tag "Projetil".
        if (other.CompareTag("Projetil"))
        {
            LevarDano(1); // O dano poderia vir do projétil no futuro.
        }
    }
}