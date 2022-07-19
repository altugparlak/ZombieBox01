using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject enemyParent;
    private bool canSpawn = true;

    private void Start()
    {
        SpawningProgress();
    }


    public void SpawningProgress()
    {
        if (canSpawn)
        {
            SpawnEnemy();
            canSpawn = false;
            StartCoroutine(WaitForShooting(3f));
        }
    }
    public void SpawnEnemy()
    {
        GameObject shoot = Instantiate(enemy, transform.position, Quaternion.identity);
        shoot.transform.SetParent(enemyParent.transform);
    }

    private IEnumerator WaitForShooting(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canSpawn = true;
        SpawningProgress();
    }
}
