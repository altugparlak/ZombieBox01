using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed = 1f;
    public GameObject hit;
    public GameObject flash;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject[] Detached;
    Rigidbody myrigidbody;

    PlayerShoot playerShoot;
    PlayerMovement playerMovement;
    private Vector3 direction;

    public float hitOffset = 0f;

    private int hitCount = 0;

    void Start()
    {
        myrigidbody = GetComponent<Rigidbody>();
        playerShoot = FindObjectOfType<PlayerShoot>();
        playerMovement = FindObjectOfType<PlayerMovement>();

        direction = playerMovement.target.position + new Vector3(0, 1f, 0) - playerShoot.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        myrigidbody.velocity = direction * speed;
        
    }
    void OnCollisionEnter(Collision collision)
    {
        //Lock all axes movement and rotation
        myrigidbody.constraints = RigidbodyConstraints.FreezeAll;
        speed = 0;

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point + contact.normal * hitOffset;

        if (hit != null)
        {
            var hitInstance = Instantiate(hit, pos, rot);
            if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
            else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
            else { hitInstance.transform.LookAt(contact.point + contact.normal); }

            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }
        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
            }
        }
        //hitCount++;
        //Debug.Log(hitCount);
        //if (hitCount == 2)
        //{
        //    Destroy(gameObject);
        //}
        Destroy(gameObject);
    }
}
