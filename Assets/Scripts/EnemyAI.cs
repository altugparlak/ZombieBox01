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

    public GameObject deathVFX;

    NavMeshAgent navMeshAgent;
    EnemySpawner enemySpawner;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerMovement>().transform;
        playerMovement = FindObjectOfType<PlayerMovement>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        playerMovement.AddEnemy(this.gameObject.transform);
        enemyHealth = 100;

    }

    [System.Obsolete]
    void Update()
    {
        navMeshAgent.SetDestination(target.position);
        if (enemyHealth <= 0)
        {
            playerMovement.RemoveEnemy(this.gameObject.transform);
            int randomNumber = Random.RandomRange(0, 3);
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
