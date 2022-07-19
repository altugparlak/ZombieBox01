using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    [SerializeField] int enemyHealth;

    private Transform target;
    private PlayerMovement playerMovement;

    public GameObject deathVFX;

    NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerMovement>().transform;
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerMovement.AddEnemy(this.gameObject.transform);
        enemyHealth = 100;

    }

    void Update()
    {
        navMeshAgent.SetDestination(target.position);
        if (enemyHealth <= 0)
        {
            playerMovement.RemoveEnemy(this.gameObject.transform);
            GameObject explotion = Instantiate(deathVFX, transform.position, Quaternion.identity);
            Destroy(explotion, 1.5f);
            Destroy(this.gameObject);
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
