using UnityEngine;
using UnityEngine.SceneManagement; // Essencial para usar o SceneManager

public class GameOverSceneController : MonoBehaviour
{
    // O m�todo Awake � chamado assim que a cena carrega, antes de qualquer Start.
    // � o lugar perfeito para resetar o estado do jogo.
    void Awake()
    {
        // Reseta a escala de tempo para 1 (velocidade normal),
        // garantindo que as anima��es desta cena funcionem.
        Time.timeScale = 1f;
    }

    // Voc� pode adicionar fun��es aqui para usar em bot�es.
    // Por exemplo, um bot�o de "Reiniciar":
    public void ReiniciarJogo()
    {
        // Certifique-se de que "TESTEJOGO" � o nome exato da sua cena principal.
        SceneManager.LoadScene("TESTEJOGO");
    }

    // Exemplo para um bot�o de "Voltar ao Menu":
    public void VoltarParaMenu()
    {
        // Certifique-se de que "Menu" � o nome exato da sua cena de menu.
        SceneManager.LoadScene("Menu");
    }
}