using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;    // Prefab of the enemy with EnemyAI and NavMeshAgent
    public int numberOfEnemies = 5;  // Number of enemies to spawn
    public float spawnRadius = 20f;  // Radius within which enemies can be spawned

    private bool mazeSpawned = false; // Flag to ensure enemies spawn after the maze is generated

    // Method called by the MazeSpawner to notify completion
    public void Init()
    {
        mazeSpawned = true;
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (!mazeSpawned)
        {
            Debug.LogWarning("Maze has not been generated yet. Enemies cannot be spawned.");
            return;
        }

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 spawnPosition = GetRandomNavMeshPosition();
            if (spawnPosition != Vector3.zero)
            {
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                Debug.Log($"Enemy spawned at {spawnPosition}");
                enemy.transform.parent = transform;
            }
            else
            {
                Debug.LogWarning($"Failed to find a valid NavMesh position for enemy {i + 1}.");
            }
        }
    }

    private Vector3 GetRandomNavMeshPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection += transform.position; // Offset by the spawner's position

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return Vector3.zero; // Return zero vector if no valid position is found
    }
}

