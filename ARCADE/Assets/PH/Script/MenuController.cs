using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems; // <<-- ADICIONE ESTA LINHA!

public class MenuController : MonoBehaviour
{
    [Header("Configurações de Cena")]
    public string nomeDaCenaDoJogo = "TESTEJOGO";

    [Header("Configurações de Som")]
    public AudioClip somIniciar;
    private AudioSource meuAudioSource;

    private bool transicaoIniciada = false;

    void Awake()
    {
        meuAudioSource = GetComponent<AudioSource>();
    }

    // ADICIONE ESTA FUNÇÃO START
    void Start()
    {
        // Garante que nenhum objeto de UI seja selecionado por padrão
        // Isso impede que a tecla "Espaço" ou "Enter" ative algum botão escondido
        EventSystem.current.SetSelectedGameObject(null);
    }

    void Update()
    {
        if (Input.anyKeyDown && !transicaoIniciada)
        {
            transicaoIniciada = true;
            StartCoroutine(CarregarCenaComSom());
        }
    }

    private IEnumerator CarregarCenaComSom()
    {
        if (meuAudioSource != null && somIniciar != null)
        {
            meuAudioSource.PlayOneShot(somIniciar);
            yield return new WaitForSeconds(somIniciar.length);
        }

        SceneManager.LoadScene(nomeDaCenaDoJogo);
    }
}