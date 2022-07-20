using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float attackRange = 5f;

    public Joystick joystick;
    public List<Transform> enemies;
    private PlayerShoot playerShoot;
    public CharacterController characterController;

    public Transform target;

    Rigidbody rb;

    float xMin;
    float xMax;

    float yMax;
    float yMin;

    void Start()
    {
        enemies = new List<Transform>();
        playerShoot = GetComponent<PlayerShoot>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (enemies.Count != 0)
        {
            GetClosestEnemy(enemies).gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
        }

        if (target!=null)
        {
            float distanceToTarget = Vector3.Distance(GetClosestEnemy(enemies).position, transform.position);
            if (distanceToTarget <= attackRange)
            {
                playerShoot.ShootingProgress();
                //Debug.Log("Attack!");
            }

        }

        transform.position = new Vector3(transform.position.x, 1.1f, transform.position.z);

        MoveWithController();
        MoveWithKeyboard();
    }

    private void MoveWithKeyboard()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            characterController.Move(direction * speed * Time.deltaTime);
        }
    }

    private void MoveWithController()
    {
        float moveHorizontal = joystick.Horizontal;
        float moveVertical = joystick.Vertical;
        Vector3 direction = new Vector3(moveHorizontal,0f,moveVertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            characterController.Move(direction * speed * Time.deltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0.5f, 0, 0.4F);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void AddEnemy(Transform enemyAI)
    {
        enemies.Add(enemyAI);
    }

    public void RemoveEnemy(Transform enemyAI)
    {
        enemies.Remove(enemyAI);
    }

    //private void FindTheClosestEnemy()
    //{
        
    //    foreach (Transform enemyAI in enemies)
    //    {
    //        distanceToTarget = Vector3.Distance(enemyAI.transform.position, transform.position);
    //        enemyAI.GetComponent<EnemyAI>().distanceToThePlayer = distanceToTarget;
    //    }

    //}

    private Transform GetClosestEnemy(List<Transform> enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        target = bestTarget;
        return bestTarget;
    }

}
