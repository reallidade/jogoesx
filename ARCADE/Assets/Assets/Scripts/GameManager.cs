using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnState { PlayerTurn, EnemyTurn }

public class GameManager : MonoBehaviour {
    public TurnState CurrentState { get; private set; }
    public List<EnemyController> enemies;
    private int enemyIndex = 0;

    void Start() {
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