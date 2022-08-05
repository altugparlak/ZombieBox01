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
    [SerializeField] public int waveAmount = 20;
    [SerializeField] public int waveAmountIncreament = 10;

    EnemySpawner enemySpawner;

    private int energyAmount;
    private int waveIndex;


    void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();

        waveIndex = 1; 
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
        waveAmount += waveAmountIncreament;
        enemySpawner.SetTheNumberOfEnemiesForTheWave();
    }


}
