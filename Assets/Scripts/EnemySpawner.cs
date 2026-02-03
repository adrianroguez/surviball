using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject enemyPrefab;
    public float timeBetweenWaves = 5f;
    public float spawnRadius = 10f;

    private int currentWaveCount = 1;

    void Start()
    {
        StartCoroutine(SpawnWaveRoutine());
    }

    IEnumerator SpawnWaveRoutine()
    {
        while (true)
        {
            for (int i = 0; i < currentWaveCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.5f); 
            }

            Debug.Log("Oleada de " + currentWaveCount + " enemigos terminada.");

            yield return new WaitForSeconds(timeBetweenWaves);

            currentWaveCount++; 
        }
    }

    void SpawnEnemy()
    {
        Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;
        
        Vector3 spawnPos = new Vector3(randomPoint.x, 1f, randomPoint.y) + transform.position;

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}