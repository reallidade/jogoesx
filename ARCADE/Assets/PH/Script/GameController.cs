using UnityEngine;
using UnityEngine.SceneManagement; // Essencial para mudar de cena

public class GameController : MonoBehaviour
{
    // Esta fun��o precisa ser 'public' para que o bot�o consiga "v�-la"
    public void EndGame()
    {
        // 2. Carrega a cena de Game Over
        SceneManager.LoadScene("GameOver");
    }

    // Voc� pode adicionar outras l�gicas do seu jogo aqui depois
    // como contar pontos, controlar o jogador, etc.
}