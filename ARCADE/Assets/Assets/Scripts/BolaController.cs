using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))] // Garante que a bola sempre terá um Rigidbody2D
public class BolaController : MonoBehaviour
{
    // --- VARIÁVEIS PÚBLICAS (Ajustáveis no Inspector de cada prefab) ---
    [Header("Atributos da Bola")]
    public int vida;
    public int bonusDeDestruicao = 5;
    public GameObject bolaMenorPrefab;
    public TextMeshPro textoVida;

    [Header("Configurações de Spawn")]
    public float forcaImpulsoInicial = 3f;

    // --- VARIÁVEIS PRIVADAS ---
    private int vidaInicialDestaBola; // Usado apenas para referência interna se necessário
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // A vida inicial é simplesmente o valor 'vida' definido no prefab.
        vidaInicialDestaBola = vida;
        AtualizarTextoVida();
    }

    // --- MÉTODO 1: CHAMADO PELO 'BallSpawnerManager' ---
    public void InicializarParaSpawner(Vector2 direcao)
    {
        gameObject.layer = LayerMask.NameToLayer("SpawningBall");
        rb.AddForce(direcao * forcaImpulsoInicial, ForceMode2D.Impulse);
        StartCoroutine(AtivarColisoesFinais());
    }

    // --- MÉTODO 2: CHAMADO QUANDO UMA BOLA MAIOR 'MORRE' E SE DIVIDE ---
    // **MUDANÇA AQUI:** Não precisamos mais receber a vida da bola nova.
    public void InicializarParaDivisao(Vector2 direcaoDoPulo)
    {
        // A vida da bola já está correta, pois foi definida no prefab.
        // O método Start() já cuidou de atualizar o texto.
        // A única coisa que precisamos fazer é aplicar o pulo.
        rb.linearVelocity = direcaoDoPulo;
    }

    // --- CORROTINA PARA ATIVAR A LAYER FINAL ---
    IEnumerator AtivarColisoesFinais()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.layer = LayerMask.NameToLayer("Bolas");
    }

    // --- LÓGICA DE DANO E MORTE ---
    public void LevarDano(int dano)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AdicionarPontos(1);
        }

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
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AdicionarPontos(bonusDeDestruicao);
        }

        if (bolaMenorPrefab != null)
        {
            // **MUDANÇA AQUI:** Removemos o cálculo da vida.

            // Bola 1 (Esquerda)
            GameObject bola1_objeto = Instantiate(bolaMenorPrefab, transform.position, Quaternion.identity);
            // Agora chamamos o método sem passar a vida.
            bola1_objeto.GetComponent<BolaController>().InicializarParaDivisao(new Vector2(-2f, 3f));

            // Bola 2 (Direita)
            GameObject bola2_objeto = Instantiate(bolaMenorPrefab, transform.position, Quaternion.identity);
            bola2_objeto.GetComponent<BolaController>().InicializarParaDivisao(new Vector2(2f, 3f));
        }

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projetil"))
        {
            LevarDano(1);
            Destroy(other.gameObject);
        }
    }
}