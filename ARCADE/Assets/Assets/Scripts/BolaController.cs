using UnityEngine;
using TMPro;

public class BolaController : MonoBehaviour
{
    // --- VARIÁVEIS PÚBLICAS ---
    public int vida; // Este é o valor ATUAL da bola, que diminui.
    public GameObject bolaMenorPrefab;
    public TextMeshPro textoVida;

    // --- VARIÁVEL DE "MEMÓRIA" ---
    private int vidaInicialDestaBola; // <<< AQUI ESTÁ A MÁGICA!

    void Start()
    {
        // Ao ser criada, a bola IMEDIATAMENTE guarda seu valor inicial.
        // Se ela foi criada com vida = 20, vidaInicialDestaBola será 20 para sempre.
        vidaInicialDestaBola = vida;

        // O resto continua como antes
        if (textoVida != null)
        {
            textoVida.text = vida.ToString();
        }
    }

    public void LevarDano(int dano)
    {
        vida -= dano;

        if (vida <= 0)
        {
            // Agora não precisamos mais passar parâmetros para a função Morrer.
            Morrer();
        }
        else
        {
            // Se não morreu, apenas atualiza o texto.
            textoVida.text = vida.ToString();
        }
    }

    private void Morrer()
    {
        if (bolaMenorPrefab != null)
        {
            // --- A GRANDE MUDANÇA ESTÁ AQUI ---
            // Usamos o valor que guardamos na memória para o cálculo!
            // Ex: vidaInicialDestaBola (20) / 2 = 10.
            int vidaDasBolasMenores = vidaInicialDestaBola / 2;

            // Garante que a vida seja no mínimo 1.
            if (vidaDasBolasMenores < 1)
            {
                vidaDasBolasMenores = 1;
            }

            // A lógica de criação das bolas continua a mesma...
            // Bola 1 (Esquerda)
            GameObject bola1_objeto = Instantiate(bolaMenorPrefab, transform.position, Quaternion.identity);
            bola1_objeto.GetComponent<BolaController>().vida = vidaDasBolasMenores;
            bola1_objeto.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2, 5), ForceMode2D.Impulse);

            // Bola 2 (Direita)
            GameObject bola2_objeto = Instantiate(bolaMenorPrefab, transform.position, Quaternion.identity);
            bola2_objeto.GetComponent<BolaController>().vida = vidaDasBolasMenores;
            bola2_objeto.GetComponent<Rigidbody2D>().AddForce(new Vector2(2, 5), ForceMode2D.Impulse);
        }

        Destroy(gameObject);
    }

    // Usando OnTriggerEnter2D para não ter impacto físico
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projetil"))
        {
            LevarDano(1);
        }
    }
}