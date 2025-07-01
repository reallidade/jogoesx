using UnityEngine;

public class pombo : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 10f;

    [Header("Tiro")]
    public GameObject projetilPrefab; // O que vamos atirar (ex: uma pena)
    public Transform pontoDeTiro;     // De onde o tiro sai
    public float taxaDeTiro = 0.2f;   // Tiros por segundo (0.2s entre cada tiro)

    private float proximoTiro = 0f; // Controla o tempo para o pr�ximo tiro

    void Update()
    {
        // --- L�GICA DE MOVIMENTO ---
        float moveInput = Input.GetAxis("Horizontal"); // Pega input do teclado (A/D, setas)
        transform.Translate(Vector2.right * moveInput * moveSpeed * Time.deltaTime);

        // --- L�GICA DE TIRO CONT�NUO ---
        if (Time.time > proximoTiro)
        {
            // Atualiza o tempo para o pr�ximo tiro
            proximoTiro = Time.time + taxaDeTiro;
            Atirar();
        }
    }

    void Atirar()
    {
        // Cria uma inst�ncia do nosso proj�til no local do pontoDeTiro
        Instantiate(projetilPrefab, pontoDeTiro.position, pontoDeTiro.rotation);
    }
}