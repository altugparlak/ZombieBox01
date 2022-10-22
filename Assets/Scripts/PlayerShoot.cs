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
    [SerializeField] AudioClip laserShootingSound;
    [SerializeField] AudioClip machineGunSound;
    public AudioSource audioSource;


    PlayerMovement playerMovement;
    PlayerHealth playerHealth;

    public bool notDeath1 = true;
    private bool canShoot = true;
    private int projectileIndex = 0;
    public float shootingWaitTime = 1.5f;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
        playerHealth = GetComponent<PlayerHealth>();
        projectile = projectiles[projectileIndex];
        notDeath1 = true;
    }
    public void ShootingProgress(Vector3 lookvector)
    {
        if (canShoot && notDeath1)
        {
            audioSource.PlayOneShot(machineGunSound);
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

    public void SwitchWeapon()
    {
        if (projectileIndex == 0)
        {
            projectileIndex = 1;
            projectile = projectiles[projectileIndex];

        }
        else
        {
            projectileIndex = 0;
            projectile = projectiles[projectileIndex];

        }
    }

    public void DualWeaponActivationProgress()
    {
        if (playerHealth.gameSession.energyUsable)
        {
            dualShot = true;
            StartCoroutine(DualWeaponDelay(5f));
        }
        else
            dualShot = false;
    }

    private IEnumerator DualWeaponDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        dualShot = false;
    }
}
