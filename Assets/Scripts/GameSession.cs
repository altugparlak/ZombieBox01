using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private Text playerMoneyTxt;

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

    [Header("Necessary Prefabs")]
    [SerializeField] public GameObject energy;
    [SerializeField] public List<GameObject> skills = new List<GameObject>();

    [Header("SkillImages&Button")]
    [SerializeField] private Image electricField;
    [SerializeField] private Image skullFear;
    [SerializeField] private Button spellButton;

    [Header("Others")]
    [SerializeField] public int randomSkillCost;
    [SerializeField] private TextMeshPro randomSkillCostText;
    [SerializeField] public int weaponUpgradeCost;
    [SerializeField] private TextMeshPro weaponUpgradeCostText;


    EnemySpawner enemySpawner;
    PlayerShoot playerShoot;

    public int playerMoney;
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
        playerShoot = FindObjectOfType<PlayerShoot>();
        gameEndWindow.SetActive(false);
        spellButton.GetComponent<Button>().interactable = false;
        //healthIncrementforZombies = (waveIndex-1) * 100;
        energyAmount = maxEnergyAmount;
        waveText.text = $"Wave {waveIndex}";

        playerMoney = 0;
        playerMoneyTxt.text = $"{playerMoney.ToString()}$";
        randomSkillCostText.text = $"{randomSkillCost.ToString()}$";
        weaponUpgradeCostText.text = $"{weaponUpgradeCost.ToString()}$";

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

    public int GetEnergyAmount()
    {
        return energyAmount;
    }

    public void GainMoney(int value)
    {
        playerMoney += value;
        playerMoneyTxt.text = $"{playerMoney.ToString()}$";
    }

    public void SpendMoney(int value)
    {
        playerMoney -= value;
        playerMoneyTxt.text = $"{playerMoney.ToString()}$";
    }

    public void PickRandomSkillandActivate()
    {
        int randomNumber = UnityEngine.Random.Range(0, 2);
        switch (randomNumber)
        {
            case 0:
                ActivateElectricFieldSkill();
                break;
            case 1:
                ActivateScareSkill();
                break;
            default:
                break;
        }
    }

    private void ActivateElectricFieldSkill()
    {
        skullFear.enabled = false;
        electricField.enabled = true;
        playerShoot.castShockWave = true;
        playerShoot.castEnemyFear = false;

        spellButton.GetComponent<Button>().interactable = true;
    }

    private void ActivateScareSkill()
    {

        skullFear.enabled = true;
        electricField.enabled = false;
        playerShoot.castEnemyFear = true;
        playerShoot.castShockWave = false;

        spellButton.GetComponent<Button>().interactable = true;
    }

    public void WeaponUpgrade()
    {
        playerShoot.projectileIndex++;
        if (playerShoot.projectileIndex >= playerShoot.projectiles.Count)
            playerShoot.projectileIndex = playerShoot.projectiles.Count - 1;

        playerShoot.projectile = playerShoot.projectiles[playerShoot.projectileIndex];
        playerShoot.currentWeaponShootingSound = playerShoot.laserShootingSound0;
    }



}
