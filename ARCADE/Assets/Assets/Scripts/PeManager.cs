using System.Collections;
using UnityEngine;

public class PeManager : MonoBehaviour
{
    [Header("Configura��es do Spawner")]
    public GameObject pePrefab;      // Arraste o prefab do p� aqui.
    public Transform[] pontosDeSpawn; // Locais onde o p� pode aparecer.

    [Header("Controle de Tempo")]
    public float tempoMinimoEntrePisadas = 5f;
    public float tempoMaximoEntrePisadas = 10f;

    void Start()
    {
        StartCoroutine(RotinaDeSpawnDePes());
    }

    IEnumerator RotinaDeSpawnDePes()
    {
        // Espera um pouco no in�cio do jogo antes da primeira pisada.
        yield return new WaitForSeconds(3f);

        // Loop infinito para continuar gerando p�s.
        while (true)
        {
            // Espera um tempo aleat�rio para a pr�xima pisada.
            float tempoDeEspera = Random.Range(tempoMinimoEntrePisadas, tempoMaximoEntrePisadas);
            yield return new WaitForSeconds(tempoDeEspera);

            // Sorteia um dos pontos de spawn.
            Transform pontoSorteado = pontosDeSpawn[Random.Range(0, pontosDeSpawn.Length)];

            // Cria uma inst�ncia do p� no local sorteado.
            Instantiate(pePrefab, pontoSorteado.position, pontoSorteado.rotation);
        }
    }
}