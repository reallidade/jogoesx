using UnityEngine;

public class Projeti : MonoBehaviour
{
    public float speed = 20f;
    public float tempoDeVida = 2f; // Em segundos

    void Start()
    {
        // Destr�i o proj�til depois de um tempo para n�o poluir a cena
        Destroy(gameObject, tempoDeVida);
    }

    void Update()
    {
        // Move o proj�til para cima
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    // Se o proj�til � um Trigger, usamos OnTriggerEnter2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se colidir com uma bola (vamos criar a tag "Bola" depois)
        if (other.CompareTag("Bola"))
        {
            // Destr�i o proj�til ao colidir
            Destroy(gameObject);
        }
    }
}