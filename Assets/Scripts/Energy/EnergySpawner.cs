using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySpawner : MonoBehaviour
{
    [SerializeField] private GameObject Energy;
    [SerializeField] private GameObject EnergyParent;

    private Vector3 spawnPosition;

    // Start is called before the first frame update

    [System.Obsolete]
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            float positionX = transform.position.x + Random.RandomRange(-9f, 10f);
            float positionY = 1f;
            float positionZ = transform.position.z + Random.RandomRange(-9f, 10f);

            spawnPosition = new Vector3(positionX, positionY, positionZ);
            GameObject energy = Instantiate(Energy, spawnPosition, Quaternion.identity);
            energy.transform.eulerAngles = new Vector3(90f, 0f, 0f);
            energy.transform.SetParent(EnergyParent.transform);
        }
    }
    

    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawCube(transform.position, new Vector3(18, 0.1f, 18));
    }
}
