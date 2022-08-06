using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    public GameObject enemy;
    public GameObject enemyParent;
    private bool canSpawn = true;
    private int numberOfEnemiesForTheWave;
    private int spawnedEnemies = 0;
    private int destroyedEneimes = 0;
    private float timeBetweenSpawns;

    GameSession gameSession;

    [System.Obsolete]
    private void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        numberOfEnemiesForTheWave = gameSession.waveAmount;
        timeBetweenSpawns = gameSession.waveSpawningSpeed;
        Invoke("SpawningProgress", 2f);
    }

    private void Update()
    {
        if (destroyedEneimes == numberOfEnemiesForTheWave)
        {
            Debug.Log("Wave is Completed!!");
            destroyedEneimes = 0;
            gameSession.SetUpTheNextWave();
        }
    }

    [System.Obsolete]
    public void SpawningProgress()
    {
        if (canSpawn)
        {
            SpawnEnemy();
            canSpawn = false;
            StartCoroutine(WaitForNextSpawn(timeBetweenSpawns));
        }
    }

    [System.Obsolete]
    public void SpawnEnemy()
    {
        int random = Random.RandomRange(0, 6);
        Vector3 spawnPosition = spawnPoints[random].position;
        GameObject shoot = Instantiate(enemy, spawnPosition, Quaternion.identity);
        shoot.transform.SetParent(enemyParent.transform);
        spawnedEnemies++;

    }

    [System.Obsolete]
    private IEnumerator WaitForNextSpawn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (spawnedEnemies == numberOfEnemiesForTheWave)
        {
            canSpawn = false;
            spawnedEnemies = 0;
            //Stop spawning
        }
        else
        {
            canSpawn = true;
        }
        SpawningProgress();
    }

    public void AddDestroyedEnemies()
    {
        destroyedEneimes++;
    }

    public void SetTheNumberOfEnemiesForTheWave()
    {
        numberOfEnemiesForTheWave = gameSession.waveAmount;
        Debug.Log("Wave Amount: " + numberOfEnemiesForTheWave);
        StartTheNextWave();

    }

    public void StartTheNextWave()
    {
        canSpawn = true;
        Invoke("SpawningProgress", 8f);
    }
}
