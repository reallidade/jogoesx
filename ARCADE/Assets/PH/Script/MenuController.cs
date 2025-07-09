using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Necess�rio para Coroutines (IEnumerator)

public class MenuController : MonoBehaviour
{
    [Header("Configura��es de Cena")]
    public string nomeDaCenaDoJogo = "TESTEJOGO"; // Nome da cena para carregar

    [Header("Configura��es de Som")]
    public AudioClip somIniciar; // Som a ser tocado ao clicar
    private AudioSource meuAudioSource;

    private bool transicaoIniciada = false;

    void Awake()
    {
        // Pega o componente AudioSource automaticamente
        meuAudioSource = GetComponent<AudioSource>();
        if (meuAudioSource == null)
        {
            // Se n�o houver um AudioSource, adiciona um para evitar erros
            meuAudioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // ESTA � A FUN��O QUE O BOT�O VAI CHAMAR
    public void IniciarJogo()
    {
        // Previne m�ltiplos cliques enquanto a transi��o acontece
        if (!transicaoIniciada)
        {
            transicaoIniciada = true;
            StartCoroutine(RotinaIniciarJogo());
        }
    }

    // A coroutine que toca o som e depois carrega a cena
    private IEnumerator RotinaIniciarJogo()
    {
        // Toca o som de in�cio, se houver um
        if (meuAudioSource != null && somIniciar != null)
        {
            meuAudioSource.PlayOneShot(somIniciar);
            // Espera o som terminar antes de mudar de cena
            yield return new WaitForSeconds(somIniciar.length);
        }

        // Carrega a cena do jogo
        SceneManager.LoadScene(nomeDaCenaDoJogo);
    }
}