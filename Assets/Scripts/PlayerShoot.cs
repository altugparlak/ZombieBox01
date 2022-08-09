using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private List<GameObject> projectiles;
    public GameObject projectile;
    public GameObject parent;

    [Header("Weapon Positions")]
    [SerializeField] private Transform singleBulletFireTransform;
    [SerializeField] private Transform dualBulletFireTransform1;
    [SerializeField] private Transform dualBulletFireTransform2;

    [Header("Others")]
    [SerializeField] private bool dualShot;
    PlayerMovement playerMovement;

    private bool canShoot = true;
    public float shootingWaitTime = 1.5f;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        projectile = projectiles[1];
    }
    public void ShootingProgress(Vector3 lookvector)
    {
        if (canShoot)
        {
            if (dualShot)
            {
                DualFire(lookvector);
                canShoot = false;
                StartCoroutine(WaitForShooting(0.2f));
            }
            else
            {
                Fire(lookvector);
                canShoot = false;
                StartCoroutine(WaitForShooting(0.2f));
            } 

        }
    }
    public void Fire(Vector3 lookv)
    {
        GameObject shoot = Instantiate(projectile, singleBulletFireTransform.position, Quaternion.identity);
        lookv.y = shoot.transform.position.y;
        Quaternion rot = Quaternion.LookRotation(lookv);
        shoot.transform.rotation = rot;
        shoot.transform.SetParent(parent.transform);
        Destroy(shoot, 3f);
    }

    public void DualFire(Vector3 lookv)
    {
        GameObject shoot0 = Instantiate(projectile, dualBulletFireTransform1.position, Quaternion.identity);
        GameObject shoot1 = Instantiate(projectile, dualBulletFireTransform2.position, Quaternion.identity);
        lookv.y = shoot0.transform.position.y;
        lookv.y = shoot1.transform.position.y;

        Quaternion rot = Quaternion.LookRotation(lookv);
        shoot0.transform.rotation = rot;
        shoot1.transform.rotation = rot;

        shoot0.transform.SetParent(parent.transform);
        shoot1.transform.SetParent(parent.transform);

        Destroy(shoot0, 3f);
        Destroy(shoot1, 3f);

    }

    private IEnumerator WaitForShooting(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canShoot = true;
    }



}
