using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public LevelManager levelManager;

    [Header("Prefabs & Points")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("Spawn Settings")]
    public float spawnRate = 2f;
    public int maxTotalEnemies = 25;  
    public int maxConcurrentEnemies = 5; 

    private int spawnedTotalCount = 0;
    private int currentActiveEnemies = 0;
    private float nextSpawnTime;

    void Update()
    {
        if (Time.time >= nextSpawnTime &&
            spawnedTotalCount < maxTotalEnemies &&
            currentActiveEnemies < maxConcurrentEnemies)
        {
            SpawnAtRandomPoint();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnAtRandomPoint()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No enemy prefab or spawn points assigned!");
            return;
        }

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedPoint = spawnPoints[randomIndex];


        GameObject newEnemy = Instantiate(enemyPrefab, selectedPoint.position, selectedPoint.rotation);


        EnemyHealth health = newEnemy.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.SetSpawner(this);
        }

        spawnedTotalCount++;
        currentActiveEnemies++;
    }

    public void EnemyDied()
    {
        currentActiveEnemies--;

        if (spawnedTotalCount >= maxTotalEnemies && currentActiveEnemies <= 0)
        {
            Debug.Log("Level Clear!");

            if (levelManager != null)
            {
                levelManager.ShowLevelComplete();
            }

            this.enabled = false;
        }
    }
}