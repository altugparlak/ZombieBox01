using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Projectile")]
    public GameObject SpeedBoostEffect;
    public GameObject parent;
    private GameObject SpeedBoostObject;
    private bool speedBoostActivated = false;

    [Header("Player")]
    [SerializeField] float speed = 1f;
    [SerializeField] float attackRange = 5f;


    [Header("Others")]
    [SerializeField] private GameObject enemyIndicator;

    public GameObject energy;
    public Joystick joystick;
    public List<Transform> enemies;
    private PlayerShoot playerShoot;
    public CharacterController characterController;
    public Transform target;

    private float angleHolder;
    private float speedvalueHolder;


    void Start()
    {
        enemies = new List<Transform>();
        playerShoot = GetComponent<PlayerShoot>();
        speedvalueHolder = speed;
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
        IsVisibleOnScreen(energy);
        if (speedBoostActivated)
        {
            SpeedBoostObject.transform.position = transform.position;
        }

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

    //private Vector3 FindEnergyPosition()
    //{
    //    float yValue = energy.transform.position.z - transform.position.z;
    //    float xValue = energy.transform.position.x - transform.position.x;

    //    Vector3 energyPosition = new Vector3(xValue, yValue, 0);
    //    angle = Mathf.Atan2(yValue, -xValue) * 180 / Mathf.PI;
    //    //Debug.Log(angle);


    //    return energyPosition;

        
    //}

    //public float GetTheAngleBetweenPlayerAndEnergy()
    //{
    //    Vector3 energyPos = FindEnergyPosition();

    //    Vector3 dir = energyPos - this.gameObject.transform.position;

    //    angle = UtilsClass.GetAngleFromVectorFloat(dir);




    //    return angle;
    //}

    public void ActivateSpeedBoost()
    {
        StartCoroutine(SpeedBoost(3f));
    }

    private IEnumerator SpeedBoost(float time)
    {
        speedBoostActivated = true;
        speed = speedvalueHolder * 2;
        
        GameObject shoot = Instantiate(SpeedBoostEffect, transform.position, Quaternion.identity);
        SpeedBoostObject = shoot;
        shoot.transform.SetParent(parent.transform);
        Destroy(shoot, time + 0.05f);
        
        yield return new WaitForSeconds(time);
        speedBoostActivated = false;
        speed = speedvalueHolder;
    }


    private float CalculateAngle(GameObject target)
    {
        float angle;
        float xDiff = target.transform.position.x - transform.position.x;
        float zDiff = target.transform.position.z - transform.position.z;

        angle = Mathf.Atan(xDiff / zDiff) * 180f / Mathf.PI;
        // tangent only returns an angle from -90 to +90.  we need to check if its behind us and adjust.
        if (zDiff < 0)
        {
            if (xDiff >= 0)
                angle += 180f;
            else
                angle -= 180f;
        }

        // this is our angle of rotation from 0->360
        float playerAngle = transform.eulerAngles.y;
        // we  need to adjust this to our -180->180 system.
        if (playerAngle > 180f)
            playerAngle = 360f - playerAngle;

        // now subtract the player angle to get our relative angle to target.
        angle -= playerAngle;

        // Make sure we didn't rotate past 180 in either direction
        if (angle < -180f)
            angle += 360;
        else if (angle > 180f)
            angle -= 360;

        // now we have our correct relative angle to the target between -180 and 180
        // Lets clamp it between -135 and 135
        Mathf.Clamp(angle, -135f, 135f);
        angleHolder = angle;
        //Debug.Log(angle);
        return angle;
    }

    public float GetAngle()
    {
        return angleHolder;
    }

    private void IsVisibleOnScreen(GameObject target)
    {
        Camera mainCam = Camera.main;
        Vector3 targetScreenPoint = mainCam.WorldToScreenPoint(target.GetComponent<Renderer>().bounds.center);

        if ((targetScreenPoint.x < 0) || (targetScreenPoint.x > Screen.width) ||
                (targetScreenPoint.y < 0) || (targetScreenPoint.y > Screen.height))
        {
            enemyIndicator.SetActive(true);
            CalculateAngle(energy);
        }
        else
        {
            enemyIndicator.SetActive(false);
        }

    }



}
