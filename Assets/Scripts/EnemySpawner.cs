using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<Wave> waves;


    private bool canSpawn = true;
    private int numberOfEnemiesForTheWave;
    private int spawnedEnemies = 0;
    private int destroyedEneimes = 0;
    private float timeBetweenSpawns;
    private List<GameObject> Zombies = new List<GameObject>(); // first we append
    private List<GameObject> Zombieshuffled = new List<GameObject>(); // then we shuffle the list

    public GameObject enemyParent;
    GameSession gameSession;
    GameObject enemy;
    Wave choosenWave;


    [System.Obsolete]
    private void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        SetTheWave();
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
        int randomEnemy = UnityEngine.Random.RandomRange(0, Zombieshuffled.Count);
        enemy = Zombieshuffled[randomEnemy];

        int random = UnityEngine.Random.RandomRange(0, 6);
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

    public void SetTheWave()
    {
        timeBetweenSpawns = gameSession.waveSpawningSpeed;

        int wave = gameSession.waveIndex - 1;
        int waveAmount;

        if (wave > 10)
        {
            waveAmount = waves[waves.Count-1].totalNumberOfZombies + gameSession.waveAmountIncrement;
            choosenWave = waves[waves.Count-1]; //Son Random Wave
            WaveRandomOperator(choosenWave);
            gameSession.waveAmountIncrement += 10;
        }
        else
        {
            waveAmount = waves[wave].totalNumberOfZombies;
            choosenWave = waves[wave];
            WaveRandomOperator(choosenWave);
        }
        numberOfEnemiesForTheWave = waveAmount;
        Debug.Log("Wave number: " + (wave + 1));
        Debug.Log("Wave Amount: " + waveAmount);
        Debug.Log(choosenWave);
        StartTheNextWave();

    }

    public void StartTheNextWave()
    {
        canSpawn = true;
        Invoke("SpawningProgress", 8f);
    }

    private void WaveRandomOperator(Wave wave)
    {
        Zombies.Clear();
        Zombieshuffled.Clear();

        float zombieCoefTotal = 0;
        for (int i = 0; i < choosenWave.zombies.Count; i++)
        {
            zombieCoefTotal += choosenWave.zombies[i].GetComponent<EnemyAI>().zombie.zdc;
            //zombieSpawnPossibility = choosenWave.zombies[i].GetComponent<EnemyAI>().zombie.zdc / zombieCoefTotal * 100;
            //Debug.Log(choosenWave.zombies[i].name + ": " + zombieSpawnPossibility);
        }
        for (int i = 0; i < choosenWave.zombies.Count; i++)
        {
            float zombieSpawnPossibility = 0;
            zombieSpawnPossibility = choosenWave.zombies[i].GetComponent<EnemyAI>().zombie.zdc / zombieCoefTotal * 100;
            //Debug.Log(choosenWave.zombies[i].name + ": " + zombieSpawnPossibility);
            //Debug.Log(choosenWave.zombies[i].name + ": " + (int)Math.Round(zombieSpawnPossibility));

            int index = 0;
            int zombieSP = (int)Math.Round(zombieSpawnPossibility);
            for (int j = 0; j < zombieSP; j++)
            {
                index++;
                Zombies.Add(choosenWave.zombies[i]);
            }
            Debug.Log(choosenWave.zombies[i].name + ": " + index);
        }

        Zombieshuffled = Zombies.OrderBy(x => Guid.NewGuid()).ToList();

    }


}
