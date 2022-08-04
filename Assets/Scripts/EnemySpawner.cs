using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    public GameObject enemy;
    public GameObject enemyParent;
    private bool canSpawn = true;

    [System.Obsolete]
    private void Start()
    {
        SpawningProgress();
    }

    [System.Obsolete]
    public void SpawningProgress()
    {
        if (canSpawn)
        {
            SpawnEnemy();
            canSpawn = false;
            StartCoroutine(WaitForShooting(3f));
        }
    }

    [System.Obsolete]
    public void SpawnEnemy()
    {
        int random = Random.RandomRange(0, 6);
        Vector3 spawnPosition = spawnPoints[random].position;
        GameObject shoot = Instantiate(enemy, spawnPosition, Quaternion.identity);
        shoot.transform.SetParent(enemyParent.transform);
    }

    [System.Obsolete]
    private IEnumerator WaitForShooting(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canSpawn = true;
        SpawningProgress();
    }
}
