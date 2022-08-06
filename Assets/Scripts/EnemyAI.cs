using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    [SerializeField] private List<GameObject> deathVFXlist;
    [SerializeField] int enemyHealth;

    private Transform target;
    private PlayerMovement playerMovement;
    private Animator animator;
    public GameObject deathVFX;

    NavMeshAgent navMeshAgent;
    EnemySpawner enemySpawner;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = FindObjectOfType<PlayerMovement>().transform;
        playerMovement = FindObjectOfType<PlayerMovement>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        playerMovement.AddEnemy(this.gameObject.transform);
        enemyHealth = 100;

    }

    void Update()
    {
        navMeshAgent.SetDestination(target.position);

        if ((target.position - transform.position).magnitude < 2)
        {
            animator.SetBool("attacking", true);
        }
        else
        {
            animator.SetBool("attacking", false);
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


}
