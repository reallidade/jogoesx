using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Configurações de Cena")]
    public string nomeDaCenaDoJogo = "TESTEJOGO"; // Mais fácil de mudar aqui do que no código

    [Header("Configurações de Som")]
    public AudioClip somHover; // Som de passar o mouse em cima
    public AudioClip somClick; // Som de clique
    private AudioSource meuAudioSource; // Referência para o nosso "alto-falante"

    // Awake é chamado antes de tudo, perfeito para pegar referências
    void Awake()
    {
        meuAudioSource = GetComponent<AudioSource>();
    }

    // --- FUNÇÕES DO JOGO ---
    public void IniciarJogo()
    {
        SceneManager.LoadScene(nomeDaCenaDoJogo);
    }

    // --- FUNÇÕES DE SOM DO BOTÃO ---
    public void TocarSomHover()
    {
        meuAudioSource.PlayOneShot(somHover);
    }

    public void TocarSomClick()
    {
        meuAudioSource.PlayOneShot(somClick);
    }
}