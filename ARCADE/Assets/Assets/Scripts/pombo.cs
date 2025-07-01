using UnityEngine;

public class pombo : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 10f;

    [Header("Tiro")]
    public GameObject projetilPrefab; // O que vamos atirar (ex: uma pena)
    public Transform pontoDeTiro;     // De onde o tiro sai
    public float taxaDeTiro = 0.2f;   // Tiros por segundo (0.2s entre cada tiro)

    private float proximoTiro = 0f; // Controla o tempo para o próximo tiro

    void Update()
    {
        // --- LÓGICA DE MOVIMENTO ---
        float moveInput = Input.GetAxis("Horizontal"); // Pega input do teclado (A/D, setas)
        transform.Translate(Vector2.right * moveInput * moveSpeed * Time.deltaTime);

        // --- LÓGICA DE TIRO CONTÍNUO ---
        if (Time.time > proximoTiro)
        {
            // Atualiza o tempo para o próximo tiro
            proximoTiro = Time.time + taxaDeTiro;
            Atirar();
        }
    }

    void Atirar()
    {
        // Cria uma instância do nosso projétil no local do pontoDeTiro
        Instantiate(projetilPrefab, pontoDeTiro.position, pontoDeTiro.rotation);
    }
}