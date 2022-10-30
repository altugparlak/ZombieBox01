using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] float rotationSpeed = 1f;

    [Header("Others")]
    [SerializeField] private GameObject enemyIndicator;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Text coinDisplayText;
    [SerializeField] private Material mymat;
    [SerializeField] private float materialIntensity;

    public GameObject energy;
    public Joystick joystick;
    public List<Transform> enemies;
    private PlayerShoot playerShoot;
    private PlayerHealth playerHealth;
    public CharacterController characterController;
    public Transform target;

    public bool notDeath = true;
    private bool shooting = false;
    private float angleHolder;
    private float speedvalueHolder;
    private int coin = 100;
    //private static Color defaultColor = new Color(1.528f, 0.855f, 0.353f, 1f);

    public GameObject chicken;


    void Start()
    {
        enemies = new List<Transform>();
        playerShoot = GetComponent<PlayerShoot>();
        playerHealth = GetComponent<PlayerHealth>();
        speedvalueHolder = speed;
        coinDisplayText.text = coin.ToString();
        chicken = FindObjectOfType<ChickenBrain>().gameObject;
        notDeath = true;
        mymat.SetColor("_EmissionColor", new Color(0, 1, 0, 1) * materialIntensity);
        Debug.Log(mymat.GetColor("_EmissionColor"));
    }

    void Update()
    {
        float distancetoChicken = Vector3.Distance(chicken.transform.position, transform.position);
        if (enemies.Count != 0)
        {
            //GetClosestEnemy(enemies).gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
            GetClosestEnemy(enemies);
        }

        if (target!=null)
        {
            float distanceToTarget = Vector3.Distance(GetClosestEnemy(enemies).position, transform.position);
            if (distanceToTarget <= attackRange)
            {
                Vector3 lookVector = GetClosestEnemy(enemies).position - transform.position;
                lookVector.y = transform.position.y;
                if (notDeath)
                {
                    Quaternion rot = Quaternion.LookRotation(lookVector);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1);
                }
                playerShoot.ShootingProgress(lookVector);
                shooting = true;
                if (distancetoChicken <= attackRange)
                {
                    chicken.GetComponent<ChickenBrain>().running = true;
                    //Debug.Log("Run Chickens!");
                }
                //Debug.Log("Attack!");
            }
            else
            {
                //SmoothlyTurnBacktoDefaultRotation();
                shooting = false;
            }

        }
        else
        {
            //SmoothlyTurnBacktoDefaultRotation();
            shooting = false;
        }

        transform.position = new Vector3(transform.position.x, 1.1f, transform.position.z);
        if (distancetoChicken > attackRange)
        {
            chicken.GetComponent<ChickenBrain>().running = false;
            //Debug.Log("Relax Chicken");
        }

        if (notDeath)
        {
            MoveWithController();
            MoveWithKeyboard();
        }


        //IsVisibleOnScreen(energy);
        if (speedBoostActivated)
        {
            SpeedBoostObject.transform.position = transform.position;
        }

        DroneColorControl();

    }

    private void MoveWithKeyboard()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(moveHorizontal, 0f, moveVertical);

        if (direction.magnitude >= 0.1f)
        {
            characterController.Move(direction.normalized * speed * Time.deltaTime);
            if (!shooting)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
            }
        }
    }

    private void MoveWithController()
    {
        float moveHorizontal = joystick.Horizontal;
        float moveVertical = joystick.Vertical;
        Vector3 direction = new Vector3(moveHorizontal,0f,moveVertical);
        
        if (direction.magnitude >= 0.1f)
        {
            characterController.Move(direction.normalized * speed * Time.deltaTime);
            if (!shooting)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
            }

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

    public void ActivateSpeedBoost()
    {
        bool enoughEnergy = playerHealth.gameSession.energyUsable;
        if (enoughEnergy)
        {
            StartCoroutine(SpeedBoost(3f));
        }
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

    private void IsVisibleOnScreen(GameObject targetIndicator)
    {
        Vector3 targetScreenPoint = mainCamera.WorldToScreenPoint(targetIndicator.GetComponent<Renderer>().bounds.center);

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

    private void SmoothlyTurnBacktoDefaultRotation()
    {
        var desiredRotQ = Quaternion.Euler(0, 0, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotQ, Time.deltaTime * 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Energy")
        {
            playerHealth.gameSession.addEnergy(2);
            Destroy(other.gameObject);
        }
    }

    public void AddCoin(int value)
    {
        coin += value;
        coinDisplayText.text = coin.ToString();
    }

    public void SpendCoin(int value)
    {
        coin -= value;
        coinDisplayText.text = coin.ToString();
    }

    private void DroneColorControl()
    {
        int currentEnergy = playerHealth.gameSession.GetEnergyAmount(); // This value changes between 0-6

        switch (currentEnergy)
        {
            case 0:
                mymat.SetColor("_EmissionColor", new Color(1, 0, 0, 1) * materialIntensity);
                break;
            case 1:
                mymat.SetColor("_EmissionColor", new Color(1, 0.2f, 0, 1) * materialIntensity);
                break;
            case 2:
                mymat.SetColor("_EmissionColor", new Color(0.8f, 0.4f, 0, 1) * materialIntensity);
                break;
            case 3:
                mymat.SetColor("_EmissionColor", new Color(0.6f, 0.6f, 0, 1) * materialIntensity);
                break;
            case 4:
                mymat.SetColor("_EmissionColor", new Color(0.4f, 0.8f, 0, 1) * materialIntensity);
                break;
            case 5:
                mymat.SetColor("_EmissionColor", new Color(0.2f, 1, 0, 1) * materialIntensity);
                break;
            case 6:
                mymat.SetColor("_EmissionColor", new Color(0, 1, 0, 1) * materialIntensity);
                break;
            default:
                break;
        }
    }



}
