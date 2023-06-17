using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Projectile")]
    public GameObject parent;
    private GameObject SpeedBoostObject;
    private bool speedBoostActivated = false;
    private ObjectPoolManager objectPoolManager;

    [Header("Player")]
    [SerializeField] float speed = 1f;
    [SerializeField] float attackRange = 5f;
    [SerializeField] float smoothRotSpeed = 10f;

    [Header("Others")]
    [SerializeField] private GameObject enemyIndicator;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Material mymat;
    [SerializeField] private float materialIntensity;
    [SerializeField] private GameObject skillPickUpEffect;

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
    //private static Color defaultColor = new Color(1.528f, 0.855f, 0.353f, 1f);

    private ChickenBrain chickenBrain;

    private NavMeshAgent navMeshAgent;
    private bool isMoving = false;
    private RaycastHit hit;
    public LayerMask groundLayer;
    public int currentEnergy=0;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemies = new List<Transform>();
        playerShoot = GetComponent<PlayerShoot>();
        playerHealth = GetComponent<PlayerHealth>();
        navMeshAgent.speed = speed;
        speedvalueHolder = navMeshAgent.speed;
        chickenBrain = FindObjectOfType<ChickenBrain>().gameObject.GetComponent<ChickenBrain>();
        notDeath = true;
        mymat.SetColor("_EmissionColor", new Color(0, 1, 0, 1) * materialIntensity);
        //Debug.Log(mymat.GetColor("_EmissionColor"));
        objectPoolManager = ObjectPoolManager.Instance;
    }

    void Update()
    {
        float distancetoChicken = Vector3.Distance(chickenBrain.transform.position, transform.position);
        if (enemies.Count != 0)
        {
            //GetClosestEnemy(enemies).gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
            GetClosestEnemy(enemies);
        }

        if (target!=null)
        {
            float distanceToTarget = Vector3.Distance(GetClosestEnemy(enemies).position, transform.position);
            Quaternion rotBeforeShooting = transform.rotation;

            if (distanceToTarget <= attackRange)
            {
                Vector3 lookVector = GetClosestEnemy(enemies).position - transform.position;
                lookVector.y = transform.position.y;
                if (notDeath)
                {
                    Quaternion rot = Quaternion.LookRotation(lookVector);
                    // Keep the X rotation unchanged
                    rot.x = transform.rotation.x;
                    rot.z = transform.rotation.z;
                    transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1);
                }
                playerShoot.ShootingProgress(lookVector);
                shooting = true;
                if (distancetoChicken <= attackRange)
                {
                    chickenBrain.running = true;
                    //Debug.Log("Run Chickens!");
                }
                //Debug.Log("Attack!");
            }
            else
            {
                SmoothlyTurnBacktoDefaultRotation(rotBeforeShooting);
                shooting = false;
            }

        }
        else
        {
            //SmoothlyTurnBacktoDefaultRotation();
            shooting = false;
        }

        //transform.position = new Vector3(transform.position.x, 1.1f, transform.position.z);
        if (distancetoChicken > attackRange)
        {
            chickenBrain.running = false;
            //Debug.Log("Relax Chicken");
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartMovement();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopMovement();
        }

        if (notDeath && isMoving)
        {
            MoveWithMouse();
        }

        if (speedBoostActivated)
        {
            SpeedBoostObject.transform.position = transform.position;
        }

        DroneColorControl();

    }

    private void StartMovement()
    {
        isMoving = true;
    }

    private void StopMovement()
    {
        isMoving = false;
    }

    private void MoveWithMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            if (hit.collider.CompareTag("Terrain"))
            {
                navMeshAgent.SetDestination(hit.point);
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
            float sqrDistanceToTarget = directionToTarget.sqrMagnitude;

            if (sqrDistanceToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = sqrDistanceToTarget;
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
        navMeshAgent.speed = speedvalueHolder * 2;

        GameObject shoot = objectPoolManager.GetSpeedBoostEffect();
        shoot.transform.position = transform.position;
        shoot.SetActive(true);
        SpeedBoostObject = shoot;

        yield return new WaitForSeconds(time);

        speedBoostActivated = false;
        navMeshAgent.speed = speedvalueHolder;

        SpeedBoostObject.SetActive(false);
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

    private void SmoothlyTurnBacktoDefaultRotation(Quaternion rot)
    {
        //var desiredRotQ = Quaternion.Euler(0, 0, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * smoothRotSpeed);
    }

    private void OnTriggerEnter(Collider other) // Skill PickUp
    {
        if (other.tag == "Energy")
        {
            int currentEnergy = playerHealth.gameSession.GetEnergyAmount();
            if (currentEnergy == 6)
            {
                Debug.Log("Energy is full!");
            }
            else if (currentEnergy == 5)
            {
                playerHealth.gameSession.addEnergy(1);
                Destroy(other.gameObject);
            }
            else
            {
                playerHealth.gameSession.addEnergy(2);
                Destroy(other.gameObject);
            }
            

        }

        //if (other.tag == "ElectricField")
        //{
        //    ShowElectricFieldImage();
        //    playerShoot.castShockWave = true;
        //    playerShoot.castEnemyFear = false;

        //    spellButton.GetComponent<Button>().interactable = true;
        //    Destroy(other.gameObject.GetComponentInParent<Transform>().gameObject);
        //}

        //if (other.tag == "Scare")
        //{
        //    ShowSkullFearImage();
        //    playerShoot.castEnemyFear = true;
        //    playerShoot.castShockWave = false;

        //    spellButton.GetComponent<Button>().interactable = true;
        //    Destroy(other.gameObject.GetComponentInParent<Transform>().gameObject);
        //}
    }

    private void DroneColorControl()
    {
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
