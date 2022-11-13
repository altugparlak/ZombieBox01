using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

//z01 dumb zombie
//z02 smart zombie

public class EnemyAI : MonoBehaviour
{
    [SerializeField] public Zombie zombie;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject coin;
    [SerializeField] private GameObject scaredVFX;
    private GameObject scaredVFXx;
    private int enemyHealth;
    private int enemyDamage;
    private Transform target;
    private PlayerMovement playerMovement;
    private Animator animator;
    public GameObject deathVFX;

    NavMeshAgent navMeshAgent;
    EnemySpawner enemySpawner;
    GameSession gameSession;
    SoundEffects soundEffects;
    

    private List<int> randomList = new List<int>();
    private List<int> shuffled;
    private bool hitCheck = false;
    private bool enemyScared = false;
    int randomNumber;


    void Start()
    {
        // Burada findOnject yapmamak lazım çünkü her zombie find ederse sıkıntı olabilir
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        PlayerMovement playerMovementCheck = FindObjectOfType<PlayerMovement>();
        if (playerMovementCheck)
        {
            target = playerMovementCheck.transform;
            playerMovement = playerMovementCheck;
            playerMovement.AddEnemy(this.gameObject.transform);
        }
        else
            Debug.Log("No Player is Found");

        enemySpawner = FindObjectOfType<EnemySpawner>();
        gameSession = FindObjectOfType<GameSession>();
        soundEffects = FindObjectOfType<SoundEffects>();

        ShuffleList();
        randomNumber = UnityEngine.Random.Range(0, 100);

        navMeshAgent.speed = zombie.movementSpeed;
        EnemyStatsSetUp();
    }

    void Update()
    {
        if (target!= null)
        {
            if (enemyScared)
                EnemyScaredAndRun();
            else
                EnemyMovementAndAttack();


            if (enemyHealth <= 0)
            {
                EnemyDeath();

            }
        }

    }

    private void EnemyScaredAndRun()
    {
        Vector3 runningPosition = (transform.position - target.position) + transform.position;
        //navMeshAgentChicken.speed = 6;
        //animator.SetBool("Scared", true);
        navMeshAgent.SetDestination(runningPosition);
        var relativePos = runningPosition - transform.position;
        var rotationVector = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationVector, Time.deltaTime * 10);
    }

    public void StartScaring()
    {
        scaredVFXx = Instantiate(scaredVFX, transform.position, Quaternion.identity);
        scaredVFXx.transform.SetParent(this.gameObject.transform);
        StartCoroutine(ScareTime(3f));
    }

    private IEnumerator ScareTime(float time)
    {
        enemyScared = true;
        yield return new WaitForSeconds(time);
        enemyScared = false;
        Destroy(scaredVFXx);
    }

    private void EnemyMovementAndAttack()
    {
        if (animator.GetBool("attacking") == false)
        {
            navMeshAgent.SetDestination(target.position);
        }


        if ((target.position - transform.position).magnitude < 2)
        {
            animator.SetBool("attacking", true);
        }
        else
        {

            if (zombie.smartAttack)
            {
                if (hitCheck)
                {
                    animator.SetBool("attacking", false);
                    hitCheck = false;
                }
            }
            else
            {
                animator.SetBool("attacking", false);

            }
        }
    }

    private void EnemyDeath()
    {
        if (shuffled[randomNumber] == 1)
        {
            CoinSpawn();
        }
        soundEffects.GetComponent<AudioSource>().PlayOneShot(zombie.zombieDeathSound);
        playerMovement.RemoveEnemy(this.gameObject.transform);
        playerMovement.GainMoney(zombie.zombieWorth);
        GameObject deathVfx = zombie.zombieDeathVFX;
        GameObject explotion = Instantiate(deathVfx, transform.position, Quaternion.identity);
        Destroy(explotion, 1.5f);
        Destroy(this.gameObject);
        enemySpawner.AddDestroyedEnemies();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ProjectileMovement>() != null)
        {
            TakeDamage(collision.gameObject.GetComponent<ProjectileMovement>().projectileDamege);
            //Debug.Log("particle");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TakeDamage(1000);
    }


    private void TakeDamage(int damage)
    {
        enemyHealth -= damage;
        
    }

    public void Hit( )
    {
        if (playerMovement!=null)
        {
            soundEffects.GetComponent<AudioSource>().PlayOneShot(zombie.zombieAttackSound);

            PlayerHealth playerHealth = playerMovement.GetComponent<PlayerHealth>();
            if (playerHealth.playerHealth > 0)
                playerHealth.losePlayerHealth(enemyDamage);
            hitCheck = true;
            GameObject hitEffect = Instantiate(hitVFX, target.position, Quaternion.identity);
            Destroy(hitEffect, 1.5f);
        }

    }

    private void EnemyStatsSetUp()
    {
        int enemyHealthIncrement = gameSession.healthIncrementforZombies;
        enemyHealth = zombie.health + enemyHealthIncrement;

        int enemyDamageIncrement = gameSession.damageIncrementforZombies;
        enemyDamage = zombie.attackDamage + enemyDamageIncrement;
    }

    private void ShuffleList()
    {
        for (int i = 0; i < 90; i++)
        {
            randomList.Add(0);
        }
        for (int i = 0; i < 10; i++)
        {
            randomList.Add(1);
        }
        //ilk element ağır mermi ikincisi hafif mermi, bunları yeni bir liste oluşturup karıştırdık.
        shuffled = randomList.OrderBy(x => Guid.NewGuid()).ToList();

    }

    private void CoinSpawn()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
        //offset
        Instantiate(coin, spawnPosition, Quaternion.identity);
        coin.transform.eulerAngles = new Vector3(90f, 0f, 0f);

    }

}
