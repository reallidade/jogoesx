using UnityEngine;
using TMPro; // Precisamos disso para usar o TextMeshPro

public class BolaController : MonoBehaviour
{
    // --- Vari�veis P�blicas (para ajustar no Inspector) ---
    public int vida = 10; // A "vida" ou o n�mero da bola
    public GameObject bolaMenorPrefab; // O Prefab da bola menor que ser� criada
    public TextMeshPro textoVida; // Refer�ncia para o texto que mostra a vida

    private Rigidbody2D rb;

    void Start()
    {
        // Pega o componente Rigidbody2D no in�cio para n�o precisar buscar toda hora
        rb = GetComponent<Rigidbody2D>();

        // Atualiza o texto da vida assim que a bola � criada
        textoVida.text = vida.ToString();
    }

    // Fun��o p�blica para que outros objetos (como o tiro) possam causar dano
    public void LevarDano(int dano)
    {
        vida -= dano;
        textoVida.text = vida.ToString(); // Atualiza o texto na tela

        if (vida <= 0)
        {
            Morrer();
        }
    }

    private void Morrer()
    {
        // Se existe um prefab de bola menor, cria duas no lugar desta
        if (bolaMenorPrefab != null)
        {
            // Cria a primeira bola menor
            GameObject bola1 = Instantiate(bolaMenorPrefab, transform.position, Quaternion.identity);
            // Joga para a esquerda
            bola1.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2, 5), ForceMode2D.Impulse);

            // Cria a segunda bola menor
            GameObject bola2 = Instantiate(bolaMenorPrefab, transform.position, Quaternion.identity);
            // Joga para a direita
            bola2.GetComponent<Rigidbody2D>().AddForce(new Vector2(2, 5), ForceMode2D.Impulse);
        }

        // Destroi a bola atual
        Destroy(gameObject);
    }

    // Detecta a colis�o com outros objetos
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se colidiu com um objeto com a tag "Projetil"
        // (Voc� precisa criar e aplicar essa tag no seu prefab de tiro)
        if (collision.gameObject.CompareTag("Projetil"))
        {
            LevarDano(1); // Causa 1 de dano
            Destroy(collision.gameObject); // Destroi o proj�til
        }
    }
}