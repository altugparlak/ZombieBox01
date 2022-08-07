using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    [SerializeField] private List<GameObject> deathVFXlist;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] int enemyHealth = 200;

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

    public void Hit( )
    {
        GameObject hitEffect = Instantiate(hitVFX, target.position, Quaternion.identity);
        Destroy(hitEffect, 1.5f);
    }

}
