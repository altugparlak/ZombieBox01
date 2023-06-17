using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Animator waveTextAnimator;
    [SerializeField] private Text waveText;
    [SerializeField] private Text waveText2;
    [SerializeField] private List<GameObject> UIList = new List<GameObject>();
    [SerializeField] private GameObject gameEndWindow;
    [SerializeField] private Text playerMoneyTxt;
    [SerializeField] private GameObject transparentImage; // for pause scene
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject quitButton;



    [Header("UI-EnergySticks")]
    [SerializeField] private List<Image> energySticks = new List<Image>();

    [Header("Wave Specs")]
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
    [SerializeField] private Image electricFieldImage;
    [SerializeField] private Image skullFearImage;
    [SerializeField] private Image fireImage;

    [SerializeField] private Button spellButton;

    [Header("Others")]
    [SerializeField] public int randomSkillCost;
    [SerializeField] public TextMeshPro randomSkillCostText;
    [SerializeField] public int weaponUpgradeCost;
    [SerializeField] public TextMeshPro weaponUpgradeCostText;
    [SerializeField] private int weaponDamageIncrement;

    EnemySpawner enemySpawner;
    PlayerShoot playerShoot;
    PlayerMovement playerMovement;

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
        playerMovement = playerShoot.GetComponent<PlayerMovement>();
        gameEndWindow.SetActive(false);
        spellButton.GetComponent<Button>().interactable = false;
        energyAmount = maxEnergyAmount;
        playerMovement.currentEnergy = energyAmount;
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
        playerMovement.currentEnergy = energyAmount;
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
        playerMovement.currentEnergy = energyAmount;
    }

    public void SetUpTheNextWave()
    {
        if (waveIndex % 5 == 0)
        {
            Debug.Log("ZombiesHealthUpgrade!");
            healthIncrementforZombies = (waveIndex - 1) * 20;
        }
        //Every 5 wave zombies should get stronger
        //Relatively to that playerWeapon should also be upgradable
        waveIndex++;
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
    
    public void WeaponUpgrade()
    {
        playerShoot.projectileIndex++;
        if (playerShoot.projectileIndex >= playerShoot.projectiles.Count)
            playerShoot.projectileIndex = playerShoot.projectiles.Count - 1;

        playerShoot.projectile = playerShoot.projectiles[playerShoot.projectileIndex];
        playerShoot.currentWeaponShootingSound = playerShoot.shootingSounds[playerShoot.projectileIndex];
        playerShoot.projectileDamage += 100;
        //weaponUpgradeCost += 500;
        //weaponUpgradeCostText.text = $"{weaponUpgradeCost.ToString()}$";

    }

    #region SkillManagement
    public void PickRandomSkillandActivate()
    {
        int randomNumber = UnityEngine.Random.Range(0, 3);
        switch (randomNumber)
        {
            case 0:
                ActivateElectricFieldSkill();
                break;
            case 1:
                ActivateScareSkill();
                break;
            case 2:
                ActivateFireSkill();
                break;
            default:
                break;
        }
        //randomSkillCost += 100;
        //randomSkillCostText.text = $"{randomSkillCost.ToString()}$";
    }

    private void ActivateElectricFieldSkill()
    {
        fireImage.enabled = false;
        skullFearImage.enabled = false;
        electricFieldImage.enabled = true;
        playerShoot.castShockWave = true;
        playerShoot.castEnemyFear = false;

        spellButton.GetComponent<Button>().interactable = true;
    }

    private void ActivateScareSkill()
    {
        fireImage.enabled = false;
        skullFearImage.enabled = true;
        electricFieldImage.enabled = false;
        playerShoot.castEnemyFear = true;
        playerShoot.castShockWave = false;

        spellButton.GetComponent<Button>().interactable = true;
    }

    private void ActivateFireSkill()
    {
        fireImage.enabled = true;
        skullFearImage.enabled = false;
        electricFieldImage.enabled = false;
        playerShoot.castEnemyFear = false;
        playerShoot.castShockWave = false;
        playerShoot.castFire = true;
        spellButton.GetComponent<Button>().interactable = true;
    }

    #endregion



    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        transparentImage.SetActive(true);
        resumeButton.SetActive(true);
        quitButton.SetActive(true);
        //isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        //isPaused = false;
        pauseButton.SetActive(true);
        transparentImage.SetActive(false);
        resumeButton.SetActive(false);
        quitButton.SetActive(false);
    }
}
