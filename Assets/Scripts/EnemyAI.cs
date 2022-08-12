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
    [SerializeField] private Zombie zombie;
    [SerializeField] private List<GameObject> deathVFXlist;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject coin;
    private int enemyHealth;
    private Transform target;
    private PlayerMovement playerMovement;
    private Animator animator;
    public GameObject deathVFX;

    NavMeshAgent navMeshAgent;
    EnemySpawner enemySpawner;
    GameSession gameSession;

    private List<int> randomList = new List<int>();
    private List<int> shuffled;
    private bool hitCheck = false;
    int randomNumber;


    void Start()
    {
        // Burada findOnject yapmamak lazım çünkü her zombie find ederse sıkıntı olabilir
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = FindObjectOfType<PlayerMovement>().transform;
        playerMovement = FindObjectOfType<PlayerMovement>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        gameSession = FindObjectOfType<GameSession>();
        playerMovement.AddEnemy(this.gameObject.transform);

        ShuffleList();
        randomNumber = UnityEngine.Random.Range(0, 100);

        navMeshAgent.speed = zombie.movementSpeed;
        EnemyHealthSetUpForTheWave();
    }

    void Update()
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

        if (enemyHealth <= 0)
        {
            if (shuffled[randomNumber] == 1)
            {
                CoinSpawn();
            }
            playerMovement.RemoveEnemy(this.gameObject.transform);
            GameObject deathVfx = deathVFXlist[0]; 
            GameObject explotion = Instantiate(deathVfx, transform.position, Quaternion.identity);
            Destroy(explotion, 1.5f);
            Destroy(this.gameObject);
            enemySpawner.AddDestroyedEnemies();
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ProjectileMovement>() != null)
        {
            TakeDamage(40);
            //Debug.Log("particle");
        }
    }

    private void TakeDamage(int damage)
    {
        enemyHealth -= damage;
        
    }

    public void Hit( )
    {
        playerMovement.GetComponent<PlayerHealth>().losePlayerHealth(100);
        hitCheck = true;
        GameObject hitEffect = Instantiate(hitVFX, target.position, Quaternion.identity);
        Destroy(hitEffect, 1.5f);
    }

    private void EnemyHealthSetUpForTheWave()
    {
        int waveHealthIncrement = gameSession.healthIncrementforZombies;
        enemyHealth = zombie.health + waveHealthIncrement;
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
