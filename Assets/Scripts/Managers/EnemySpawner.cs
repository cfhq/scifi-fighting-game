using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Player player;
    public float spawnRate = 3f;
    public float minSpawnDistance = 3f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnRate);
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos;

        do
        {
            spawnPos = new Vector2(
                Random.Range(-8f, 8f),
                Random.Range(-4f, 4f)
            );
        }
        while (Vector2.Distance(spawnPos, player.transform.position) < minSpawnDistance);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}

