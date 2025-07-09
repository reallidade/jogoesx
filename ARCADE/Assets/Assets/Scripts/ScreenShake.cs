using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    // Padr�o Singleton para ser facilmente acessado de qualquer lugar.
    public static ScreenShake Instance;

    private Vector3 posicaoOriginal;
    private Coroutine shakeCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Guarda a posi��o original da c�mera no in�cio.
        posicaoOriginal = transform.localPosition;
    }

    // M�todo p�blico que outros scripts chamar�o para iniciar o tremor.
    public void IniciarTremor(float duracao, float magnitude)
    {
        // Se j� estiver tremendo, para a rotina antiga antes de come�ar uma nova.
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(Tremer(duracao, magnitude));
    }

    private IEnumerator Tremer(float duracao, float magnitude)
    {
        float tempoPassado = 0f;

        while (tempoPassado < duracao)
        {
            // Gera um deslocamento aleat�rio dentro de um c�rculo.
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Aplica o deslocamento � posi��o original da c�mera.
            transform.localPosition = posicaoOriginal + new Vector3(x, y, 0);

            tempoPassado += Time.deltaTime;

            yield return null; // Espera o pr�ximo frame.
        }

        // Ao final do tremor, garante que a c�mera volte exatamente para a posi��o original.
        transform.localPosition = posicaoOriginal;
        shakeCoroutine = null;
    }
}