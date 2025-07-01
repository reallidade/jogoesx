using UnityEngine;
using TMPro;

public class BolaController : MonoBehaviour
{
    // --- VARI�VEIS P�BLICAS ---
    public int vida; // Este � o valor ATUAL da bola, que diminui.
    public GameObject bolaMenorPrefab;
    public TextMeshPro textoVida;

    // --- VARI�VEL DE "MEM�RIA" ---
    private int vidaInicialDestaBola; // <<< AQUI EST� A M�GICA!

    void Start()
    {
        // Ao ser criada, a bola IMEDIATAMENTE guarda seu valor inicial.
        // Se ela foi criada com vida = 20, vidaInicialDestaBola ser� 20 para sempre.
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
            // Agora n�o precisamos mais passar par�metros para a fun��o Morrer.
            Morrer();
        }
        else
        {
            // Se n�o morreu, apenas atualiza o texto.
            textoVida.text = vida.ToString();
        }
    }

    private void Morrer()
    {
        if (bolaMenorPrefab != null)
        {
            // --- A GRANDE MUDAN�A EST� AQUI ---
            // Usamos o valor que guardamos na mem�ria para o c�lculo!
            // Ex: vidaInicialDestaBola (20) / 2 = 10.
            int vidaDasBolasMenores = vidaInicialDestaBola / 2;

            // Garante que a vida seja no m�nimo 1.
            if (vidaDasBolasMenores < 1)
            {
                vidaDasBolasMenores = 1;
            }

            // A l�gica de cria��o das bolas continua a mesma...
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

    // Usando OnTriggerEnter2D para n�o ter impacto f�sico
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projetil"))
        {
            LevarDano(1);
        }
    }
}