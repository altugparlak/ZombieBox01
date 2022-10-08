using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Animator animator;
    [SerializeField] private Text energyText;
    [SerializeField] private Text waveText;

    [Header("Wave Specs")]
    //[SerializeField] public int waveAmount = 20;
    //[SerializeField] public int waveAmountIncreament = 10;
    [Tooltip("Lower the number, faster the speed")]
    [SerializeField] public float waveSpawningSpeed = 3f;

    [Header("Enemy Specs")]
    [SerializeField] public int healthIncrementforZombies = 0;


    EnemySpawner enemySpawner;

    private int energyAmount;
    public int waveIndex;

    private void Awake()
    {
        waveIndex = 1;

    }
    void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();

        //healthIncrementforZombies = (waveIndex-1) * 100;
        energyAmount = 100;
        energyText.text = energyAmount.ToString();
        waveText.text = $"Wave {waveIndex}";

        Invoke("SplashWaveImageShowUp", 2f);

    }

    public void useEnergy(int amount)
    {
        energyAmount -= amount;
        energyText.text = energyAmount.ToString();

    }

    private void SplashWaveImageShowUp()
    {
        animator.SetTrigger("ShowUp");
    }

    public void SetUpTheNextWave()
    {
        waveIndex++;
        //healthIncrementforZombies = (waveIndex - 1) * 100;
        waveText.text = $"Wave {waveIndex}";
        //waveAmount += waveAmountIncreament;
        enemySpawner.SetTheWave();
        Invoke("SplashWaveImageShowUp", 2f);
    }


}
