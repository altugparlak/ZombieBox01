using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerCCW : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 5f;

    void Update()
    {
        transform.Rotate(0, -6.0f * rotateSpeed * Time.deltaTime, 0);
    }
}
