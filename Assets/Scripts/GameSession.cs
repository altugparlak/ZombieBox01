using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [Header("UI")]
    //[SerializeField] private Animator animator;
    [SerializeField] private Animator waveTextAnimator;
    [SerializeField] private Text waveText;
    [SerializeField] private Text waveText2;
    [SerializeField] private List<GameObject> UIList = new List<GameObject>();
    [SerializeField] private GameObject gameEndWindow;

    [Header("UI-EnergySticks")]
    [SerializeField] private List<Image> energySticks = new List<Image>();

    [Header("Wave Specs")]
    //[SerializeField] public int waveAmount = 20;
    //[SerializeField] public int waveAmountIncreament = 10;
    [Tooltip("Lower the number, faster the speed")]
    [SerializeField] public float waveSpawningSpeed = 3f;
    [SerializeField] public int waveAmountIncrement = 10;

    [Header("Enemy Specs")]
    [SerializeField] public int healthIncrementforZombies = 0;
    [SerializeField] public int damageIncrementforZombies = 0;

    EnemySpawner enemySpawner;

    private const int maxEnergyAmount = 6;
    private int energyAmount;
    public int waveIndex;
    public bool energyUsable = true;

    private void Awake()
    {
        waveIndex = 1;

    }
    void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        gameEndWindow.SetActive(false);
        //healthIncrementforZombies = (waveIndex-1) * 100;
        energyAmount = maxEnergyAmount;
        waveText.text = $"Wave {waveIndex}";

        foreach (Image stick in energySticks)
        {
            stick.enabled = true;
        }

    }

    public void useEnergy(int amount)
    {
        int usableEnergy = energyAmount - amount;

        if (usableEnergy >= 0)
        {
            energyUsable = true;
            for (int i = energyAmount - amount; i < energyAmount; i++)
            {
                energySticks[i].enabled = false;
            }
            energyAmount -= amount;

        }
        else
        {
            energyUsable = false;
            Debug.Log("Not Enough Energy!");
        }

    }

    public void addEnergy(int amount)
    {
        energyAmount += amount;

        if (energyAmount > maxEnergyAmount)
            energyAmount = maxEnergyAmount;

        for (int i = 0; i < energyAmount; i++)
        {
            energySticks[i].enabled = true;
        }
    }

    public void SetUpTheNextWave()
    {
        waveIndex++;
        //healthIncrementforZombies = (waveIndex - 1) * 100;
        waveText.text = $"Wave {waveIndex}";
        waveText2.text = $"Wave {waveIndex}";

        //waveAmount += waveAmountIncreament;
        enemySpawner.SetTheWave();
        waveTextAnimator.SetTrigger("WaveCompleted");
    }

    public void ActivateGameEndWindow()
    {
        foreach (var item in UIList)
        {
            item.SetActive(false);
        }

        gameEndWindow.SetActive(true);
    }


}
