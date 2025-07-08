using UnityEngine;

// Adicione esta linha para garantir que o Rigidbody2D sempre exista
[RequireComponent(typeof(Rigidbody2D))]
public class pombo : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 10f;

    [Header("Tiro")]
    public GameObject projetilPrefab;
    public Transform pontoDeTiro;
    public float taxaDeTiro = 0.2f; // Ajustei pra um valor mais razo�vel, 0.001 � muito r�pido

    private float proximoTiro = 0f;
    private Rigidbody2D rb; // Vari�vel para guardar o Rigidbody

    // Use o Awake para pegar componentes, � mais seguro que Start
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // --- L�GICA DE TIRO (pode ficar no Update) ---
        if (Time.time > proximoTiro)
        {
            proximoTiro = Time.time + taxaDeTiro;
            Atirar();
        }
    }

    // Use FixedUpdate para tudo que envolve f�sica (movimento, for�as, etc)
    void FixedUpdate()
    {
        // --- L�GICA DE MOVIMENTO ---
        float moveInput = Input.GetAxis("Horizontal");

        // Criamos o vetor de velocidade
        Vector2 novaVelocidade = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Aplicamos a velocidade ao Rigidbody
        rb.linearVelocity = novaVelocidade;
    }

    void Atirar()
    {
        Instantiate(projetilPrefab, pontoDeTiro.position, pontoDeTiro.rotation);
    }
    // Adicione este método ao seu script pombo.cs

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o objeto com que colidiu está na layer "Bolas"
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bolas"))
        {
            Debug.Log("Pombo foi atingido! Fim de jogo!");

            // Desativa o movimento do pombo para evitar mais colisões
            this.enabled = false;

            // Avisa o GameManager para iniciar a sequência de Game Over
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartGameOver();
            }
        }
    }
}