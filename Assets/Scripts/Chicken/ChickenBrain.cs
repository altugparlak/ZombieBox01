using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenBrain : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private ChickenPath chickenPath;

    Transform playerTransform;
    public Transform target;

    NavMeshAgent navMeshAgentChicken;
    public Animator animator;

    public bool moving = false;
    public bool running = false;

    private bool checkingForRunningPosition = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgentChicken = GetComponent<NavMeshAgent>();

        PlayerMovement playerMovementCheck = FindObjectOfType<PlayerMovement>();
        if (playerMovementCheck)
            playerTransform = playerMovementCheck.transform;
        else
        {
            Debug.Log("No Player is Found!");
        }
    }

    [System.Obsolete]
    void Update()
    {
        if (running && playerTransform)
        {
            Vector3 runningPosition = (transform.position - playerTransform.position) + transform.position;
            navMeshAgentChicken.speed = 6;
            animator.SetBool("Scared", true);
            navMeshAgentChicken.SetDestination(runningPosition);
            var relativePos = runningPosition - transform.position;
            var rotationVector = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationVector, Time.deltaTime * 10);
        }
        else
        {
            animator.SetBool("Scared", false);
            navMeshAgentChicken.speed = 3;
            if (moving)
            {
                animator.SetBool("Walking", true);
                //var step = movementSpeed * Time.deltaTime; // calculate distance to move
                navMeshAgentChicken.SetDestination(target.position);
                //transform.position = Vector3.MoveTowards(transform.position, target.position, step);

                var relativePos = target.position - transform.position;
                var rotationVector = Quaternion.LookRotation(relativePos);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotationVector, Time.deltaTime * 5);
            }
            else
            {
                animator.SetBool("Walking", false);
            }
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "ChickenPosition")
        {
            moving = false;
        }

    }


}
