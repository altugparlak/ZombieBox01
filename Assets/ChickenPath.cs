using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenPath : MonoBehaviour
{
    [SerializeField] private float xLength;
    [SerializeField] private float zLength;
    [SerializeField] private GameObject newPositionGameObj;
    [SerializeField] private ChickenBrain chickenBrain;

    private int timeHolder = 0;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        CreateNewPosition();
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {

        if (Time.time > timeHolder)
        {
            CreateNewPosition();
        }
    }

    [System.Obsolete]
    private void CreateNewPosition()
    {
        timeHolder += 5;
        Vector3 newPosition;
        float positionX = transform.position.x + Random.RandomRange(-(xLength / 2), (xLength / 2));
        float positionY = 1f;
        float positionZ = transform.position.z + Random.RandomRange(-(xLength / 2), (xLength / 2));
        newPosition = new Vector3(positionX, positionY, positionZ);
        newPositionGameObj.transform.position = newPosition;
        chickenBrain.target = newPositionGameObj.transform;
        chickenBrain.moving = true;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawCube(transform.position, new Vector3(xLength, 0.1f, zLength));
    }
}
