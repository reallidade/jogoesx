using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    // --- PADR�O SINGLETON ---
    // Permite acesso f�cil e global atrav�s de "FadeManager.Instance".
    public static FadeManager Instance;

    [Header("Configura��o")]
    // Arraste o objeto "FadeScreen" (que tem o Animator) aqui pelo Inspector.
    public Animator animator;
    // Dura��o do fade, deve ser igual ao tempo das suas anima��es de fade.
    public float tempoDeFade = 1f;

    // Awake � chamado antes de qualquer Start, ideal para configurar o Singleton.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // A M�GICA: Este objeto n�o ser� destru�do ao carregar uma nova cena.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Evita duplicatas do FadeManager se voc� voltar para a cena principal.
            Destroy(gameObject);
        }
    }

    // O m�todo Start � chamado uma vez quando o FadeManager � criado.
    private void Start()
    {
        // Se o FadeScreen come�ar desativado, esta rotina n�o far� nada vis�vel,
        // o que � o comportamento esperado. Se ele come�ar ativo e transparente,
        // teremos um fade-in suave no in�cio do jogo.
        if (animator != null && animator.gameObject.activeInHierarchy)
        {
            animator.SetTrigger("StartFadeIn");
        }
    }

    // M�todo p�blico que qualquer script (como o GameManager) pode chamar para trocar de cena.
    public void FadeParaCena(string nomeDaCena)
    {
        // Inicia a rotina que faz a transi��o completa.
        StartCoroutine(RotinaDeFade(nomeDaCena));
    }

    // A corrotina que orquestra todo o processo de fade e carregamento de cena.
    private IEnumerator RotinaDeFade(string nomeDaCena)
    {
        // --- PARTE 1: FADE-OUT ---

        // **NOVO E IMPORTANTE:** Garante que o objeto FadeScreen esteja ativo antes de us�-lo.
        // Isso permite que voc� deixe o FadeScreen desativado por padr�o na cena.
        if (animator != null && !animator.gameObject.activeInHierarchy)
        {
            animator.gameObject.SetActive(true);
        }

        // 1. Dispara o gatilho para a anima��o de fade-out (escurecer a tela).
        animator.SetTrigger("StartFadeOut");

        // 2. Espera a anima��o de fade-out terminar.
        // Usamos WaitForSecondsRealtime para funcionar mesmo com Time.timeScale = 0.
        yield return new WaitForSecondsRealtime(tempoDeFade);

        // --- PARTE 2: CARREGAMENTO DA CENA ---

        // 3. Carrega a nova cena. A tela ainda est� preta neste ponto porque o
        //    FadeManager (e seu filho FadeScreen) n�o foram destru�dos.
        SceneManager.LoadScene(nomeDaCena);

        // --- PARTE 3: FADE-IN ---

        // 4. Dispara o gatilho para a anima��o de fade-in (revelar a nova cena).
        animator.SetTrigger("StartFadeIn");

        // 5. Espera a anima��o de fade-in terminar.
        yield return new WaitForSecondsRealtime(tempoDeFade);

        // Opcional: Desativar a tela de fade depois de tudo para economizar performance,
        // embora para um objeto simples como este, n�o seja estritamente necess�rio.
        // animator.gameObject.SetActive(false);
    }
}