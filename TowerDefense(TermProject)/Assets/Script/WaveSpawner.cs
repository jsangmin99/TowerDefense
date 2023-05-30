using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform spawnPoint;
    public float timeBetweenEnemies = 1f;
    public int numberOfEnemies = 10; // 스폰할 총 적 수

    private int enemiesSpawned = 0;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (enemiesSpawned < numberOfEnemies)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }

    private void SpawnEnemy()
    {
        Transform enemyTransform = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        enemy.OnDestroyed += OnEnemyDestroyed;
        enemiesSpawned++;
    }

    private void OnEnemyDestroyed(Enemy enemy)
    {
        // enemy 파괴 시 수행할 동작 작성
    }
}
