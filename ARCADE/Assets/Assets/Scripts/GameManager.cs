using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnState { PlayerTurn, EnemyTurn }

public class GameManager : MonoBehaviour {
    public TurnState CurrentState { get; private set; }
    public List<EnemyController> enemies;
    private int enemyIndex = 0;
    public GameObject bolaInicialPrefab; // Arraste o prefab da Bola grande aqui
    public Transform pontoDeSpawn; // Crie um objeto vazio na cena para ser o ponto de spawn


    void Start() {
        // Cria a primeira bola no início do jogo
        GameObject bola = Instantiate(bolaInicialPrefab, pontoDeSpawn.position, Quaternion.identity);

        // Dá um impulso inicial para ela não cair reto
        bola.GetComponent<Rigidbody2D>().AddForce(new Vector2(5, 0), ForceMode2D.Impulse);
        StartPlayerTurn();
    }

    public void StartPlayerTurn() {
        CurrentState = TurnState.PlayerTurn;
    }

    public void EndPlayerTurn() {
        CurrentState = TurnState.EnemyTurn;
        enemyIndex = 0;
        StartCoroutine(ProcessEnemies());
    }

    IEnumerator ProcessEnemies() {
        while (enemyIndex < enemies.Count) {
            enemies[enemyIndex].PerformAction(FindObjectOfType<PlayerController>());
            enemyIndex++;
            yield return new WaitForSeconds(0.5f);
        }
        StartPlayerTurn();
    }

    public void EndEnemyTurn() {
        // Logic handled in coroutine
    }
}
