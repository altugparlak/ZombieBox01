using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//z01 dumb zombie
//z02 smart zombie

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Zombie zombie;
    [SerializeField] private List<GameObject> deathVFXlist;
    [SerializeField] private GameObject hitVFX;
    private int enemyHealth;
    private Transform target;
    private PlayerMovement playerMovement;
    private Animator animator;
    public GameObject deathVFX;

    NavMeshAgent navMeshAgent;
    EnemySpawner enemySpawner;
    GameSession gameSession;

    private bool hitCheck = false;

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
            playerMovement.RemoveEnemy(this.gameObject.transform);
            int randomNumber = 0;
            GameObject deathVfx = deathVFXlist[randomNumber]; 
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
        hitCheck = true;
        GameObject hitEffect = Instantiate(hitVFX, target.position, Quaternion.identity);
        Destroy(hitEffect, 1.5f);
    }

    private void EnemyHealthSetUpForTheWave()
    {
        int waveHealthIncrement = gameSession.healthIncrementforZombies;
        enemyHealth = zombie.health + waveHealthIncrement;
    }

}
