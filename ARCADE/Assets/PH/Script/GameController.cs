using UnityEngine;
using UnityEngine.SceneManagement; // Essencial para mudar de cena

public class GameController : MonoBehaviour
{
    // Esta fun��o precisa ser 'public' para que o bot�o consiga "v�-la"
    public void EndGame()
    {
        // 1. Simula uma pontua��o aleat�ria e a guarda na vari�vel est�tica
        // Lembre-se que seu script se chama 'GameOverUI', ent�o usamos ele aqui.
        GameOverUI.GameSession.score = Random.Range(100, 10000);

        // 2. Carrega a cena de Game Over
        SceneManager.LoadScene("GameOver");
    }

    // Voc� pode adicionar outras l�gicas do seu jogo aqui depois
    // como contar pontos, controlar o jogador, etc.
}