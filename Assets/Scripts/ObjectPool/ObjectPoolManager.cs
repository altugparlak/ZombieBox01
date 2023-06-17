using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    public GameObject speedBoostEffectPrefab;
    public int initialPoolSize = 5;

    private List<GameObject> speedBoostEffectPool = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        InitializeObjectPool();
    }

    private void InitializeObjectPool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject speedBoostEffect = Instantiate(speedBoostEffectPrefab);
            speedBoostEffect.SetActive(false);
            speedBoostEffectPool.Add(speedBoostEffect);
        }
    }

    public GameObject GetSpeedBoostEffect()
    {
        foreach (GameObject speedBoostEffect in speedBoostEffectPool)
        {
            if (!speedBoostEffect.activeInHierarchy)
                return speedBoostEffect;
        }

        // If there are no inactive speed boost effects in the pool, create a new one
        GameObject newSpeedBoostEffect = Instantiate(speedBoostEffectPrefab);
        speedBoostEffectPool.Add(newSpeedBoostEffect);
        return newSpeedBoostEffect;
    }
}
