using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Configura��es de Cena")]
    public string nomeDaCenaDoJogo = "TESTEJOGO"; // Mais f�cil de mudar aqui do que no c�digo

    [Header("Configura��es de Som")]
    public AudioClip somHover; // Som de passar o mouse em cima
    public AudioClip somClick; // Som de clique
    private AudioSource meuAudioSource; // Refer�ncia para o nosso "alto-falante"

    // Awake � chamado antes de tudo, perfeito para pegar refer�ncias
    void Awake()
    {
        meuAudioSource = GetComponent<AudioSource>();
    }

    // --- FUN��ES DO JOGO ---
    public void IniciarJogo()
    {
        SceneManager.LoadScene(nomeDaCenaDoJogo);
    }

    // --- FUN��ES DE SOM DO BOT�O ---
    public void TocarSomHover()
    {
        meuAudioSource.PlayOneShot(somHover);
    }

    public void TocarSomClick()
    {
        meuAudioSource.PlayOneShot(somClick);
    }
}