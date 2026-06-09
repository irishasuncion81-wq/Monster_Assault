using UnityEngine;
using System.Collections;

public class EnemySpawnClassic : MonoBehaviour
{
    [System.Serializable]
    public class EnemyGroup
    {
        public string groupName;
        public GameObject enemyPrefab;
        public Transform[] spawnPoints;
        public float spawnDelay = 1.0f;
    }

    [Header("Enemy Groups Setup")]
    public EnemyGroup groundEnemies;
    public EnemyGroup airEnemies;
    public EnemyGroup waterEnemies;

    [Header("Spawn Control Settings")]
    public int maxEnemiesBeforeStop = 8; // Pag umabot sa 8, stop muna
    public int resumeThreshold = 3;      // Pag 3 na lang, spawn uli
    public int enemiesToSpawnPerBatch = 5; // 5 ang i-spawn na bago
    public float checkInterval = 2.0f;   // Gaano kadalas i-check ang enemy count

    private bool isSpawning = false;

    void Start()
    {
        // Sisimulan ang loop na nagbabantay sa dami ng enemy
        StartCoroutine(MonitorEnemyCount());
    }

    IEnumerator MonitorEnemyCount()
    {
        while (true)
        {
            // Bilangin kung ilan ang "Enemy" sa buong scene
            int currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

            // Logic: Kung konti na lang ang enemy (3 or less) at hindi kasalukuyang nag-i-spawn
            if (currentEnemyCount <= resumeThreshold && !isSpawning)
            {
                // Siguraduhin na hindi lalampas sa max limit pag nag-spawn ng 5
                if (currentEnemyCount + enemiesToSpawnPerBatch <= 15) // Safety buffer
                {
                    StartCoroutine(SpawnNewBatch());
                }
            }

            // Mag-antay ng konti bago magbilang ulit para hindi laggy
            yield return new WaitForSeconds(checkInterval);
        }
    }

    IEnumerator SpawnNewBatch()
    {
        isSpawning = true;
        Debug.Log("Enemies are low! Spawning 5 new enemies...");

        for (int i = 0; i < enemiesToSpawnPerBatch; i++)
        {
            // Randomly pumili kung Ground, Air, o Water ang ilalabas
            int randomType = Random.Range(0, 3);
            EnemyGroup selectedGroup;

            if (randomType == 0) selectedGroup = groundEnemies;
            else if (randomType == 1) selectedGroup = airEnemies;
            else selectedGroup = waterEnemies;

            SpawnSingleEnemy(selectedGroup);

            // Delay sa pagitan ng 5 enemies para hindi sabay-sabay sa isang spot
            yield return new WaitForSeconds(0.5f);
        }

        isSpawning = false;
    }

    void SpawnSingleEnemy(EnemyGroup group)
    {
        if (group.enemyPrefab == null || group.spawnPoints.Length == 0) return;

        int randomPoint = Random.Range(0, group.spawnPoints.Length);
        Instantiate(group.enemyPrefab, group.spawnPoints[randomPoint].position, Quaternion.identity);
    }
}