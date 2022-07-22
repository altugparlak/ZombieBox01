using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float padding = 1f;
    [SerializeField] float moveSpeed = 5f;
    RectTransform rectTransform;

    PlayerMovement playerMovement;
    Camera gameCamera;


    float xMin = -600;
    float xMax = 600;

    float yMax = 300;
    float yMin = -300;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        gameCamera = Camera.main;
        rectTransform.anchoredPosition = new Vector3(-600, 300, 0);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float theAngle = playerMovement.GetTheAngleBetweenPlayerAndEnergy();

        // theAnge 180-----0
        //        -180-----0

        float xValue;
        float yValue;

        float controlledXvalue;
        float controlledYvalue;
        if (theAngle >= 0)
        {
            if (theAngle < 25)
            {
                yValue = theAngle / 25;
                controlledYvalue = Mathf.Clamp(yValue, 0.5f, 0.9f);
                transform.position = gameCamera.ViewportToScreenPoint(new Vector3(0.1f, controlledYvalue, 0));
            }
            else if (theAngle >= 25 && theAngle <= 155)
            {
                xValue = theAngle / 180;
                controlledXvalue = Mathf.Clamp(xValue, 0.1f, 0.9f);
                transform.position = gameCamera.ViewportToScreenPoint(new Vector3(controlledXvalue, 0.9f, 0));
            }
            else if (theAngle > 155)
            {
                yValue = 156 / theAngle; // Şuanda burası sıkıntılı
                Debug.Log(yValue);
                controlledYvalue = Mathf.Clamp(yValue, 0.5f, 0.9f);
                transform.position = gameCamera.ViewportToScreenPoint(new Vector3(0.9f, controlledYvalue, 0));
            }
        }

    }

}
