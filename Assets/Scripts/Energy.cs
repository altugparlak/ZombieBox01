using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{

    [SerializeField] private float rotateSpeed = 1f;


    private void Start()
    {
        transform.eulerAngles = new Vector3(90f, 0f, 0f);
    }

    private void Update()
    {
        transform.Rotate(0, 0, 6.0f * rotateSpeed * Time.deltaTime);
    }


}
