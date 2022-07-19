using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    public GameObject projectile;
    public GameObject parent;

    private bool canShoot = true;
    public float shootingWaitTime = 1.5f;

    public void ShootingProgress()
    {
        if (canShoot)
        {
            Fire();
            canShoot = false;
            StartCoroutine(WaitForShooting(0.2f));
        }
    }
    public void Fire()
    {
        GameObject shoot = Instantiate(projectile, transform.position, Quaternion.identity);
        shoot.transform.SetParent(parent.transform);
        Destroy(shoot, 3f);
    }

    private IEnumerator WaitForShooting(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canShoot = true;
    }



}
