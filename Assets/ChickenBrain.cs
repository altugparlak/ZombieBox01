using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenBrain : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    public Transform target;

    NavMeshAgent navMeshAgentChicken;
    Animator animator;

    public bool moving = false;



    private void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgentChicken = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "ChickenPosition")
        {
            moving = false;
        }

    }


}
