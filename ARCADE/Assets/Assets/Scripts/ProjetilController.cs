using UnityEngine;

public class ProjetilController : MonoBehaviour
{
    // --- Vari�veis P�blicas ---
    public float velocidade = 20f;   // Velocidade com que o proj�til sobe
    public float tempoDeVida = 3f;   // Tempo em segundos antes de se autodestruir

    void Start()
    {
        // Garante que o proj�til ser� destru�do depois de um tempo,
        // para n�o poluir a cena com objetos infinitos.
        Destroy(gameObject, tempoDeVida);
    }

    void Update()
    {
        // Move o proj�til para cima constantemente
        transform.Translate(Vector2.up * velocidade * Time.deltaTime);
    }

    // A m�gica acontece aqui!
    // Esta fun��o � chamada automaticamente pela Unity quando este objeto colide com outro.
    // O script da bola (BolaController) j� tem a l�gica para receber o dano.
    // N�s s� precisamos garantir que o proj�til seja destru�do no impacto.
    // Substitu�mos OnCollisionEnter2D por OnTriggerEnter2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se o proj�til entrar em qualquer trigger/collider (como o da bola), ele se destr�i.
        // A gente pode at� adicionar uma verifica��o pra n�o se destruir em outros triggers se precisar no futuro.
        if (other.CompareTag("Bola")) // Adicionando tag na bola para ser mais espec�fico
        {
            Destroy(gameObject);
        }
    }
}
