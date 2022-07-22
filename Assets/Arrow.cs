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
        float theAngle = playerMovement.GetTheAngleBetweenPlayerAndEnergy();
        //Move();
        PlaceSprite(theAngle);
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
                yValue = 155.1f / theAngle; // Şuanda burası sıkıntılı
                Debug.Log(yValue);
                controlledYvalue = Mathf.Clamp(yValue, 0.5f, 0.9f);
                transform.position = gameCamera.ViewportToScreenPoint(new Vector3(0.9f, controlledYvalue, 0));
            }
        }
        else
        {
            if (theAngle < -155)
            {
                yValue = -155 / theAngle;
                controlledYvalue = Mathf.Clamp(yValue, 0.1f, 0.5f);
                transform.position = gameCamera.ViewportToScreenPoint(new Vector3(0.9f, controlledYvalue, 0));
            }
        }
    }
    //hyi birleştir
    //h ttps://forum.unity.com/threads/3d-game-offscreen-target-indicator.431342/
    private void PlaceSprite(float angle)
    {
        // Get half the Images width and height to adjust it off the screen edge;
        RectTransform arrowRect = this.GetComponent<RectTransform>();
        float halfImageWidth = arrowRect.sizeDelta.x / 2f;
        float halfImageHeight = arrowRect.sizeDelta.y / 2f;

        // Get Half the ScreenHeight and Width to position the image
        float halfScreenWidth = (float)Screen.width / 2f;
        float halfScreenHeight = (float)Screen.height / 2f;

        float xPos = 0f;
        float yPos = 0f;

        // Left side of screen;
        if (angle < -45)
        {
            xPos = -halfScreenWidth + halfImageWidth;
            // Ypos can go between +ScreenHeight/2  down to -ScreenHeight/2
            // angle goes between -45 and -135
            // change angle to a value between 0f and 1.0f and Lerp on that
            float normalizedAngle = (angle + 45f) / -90f;
            yPos = Mathf.Lerp(halfScreenHeight, -halfScreenHeight, normalizedAngle);
            // at the top of the screen we need to move the image down half its height
            // at the bottom of the screen we need to move it up half its height
            // in the middle we need to do nothing. so we lerp on the angle again
            float yImageOffset = Mathf.Lerp(-halfImageHeight, halfImageHeight, normalizedAngle);
            yPos += yImageOffset;

        }
        // top of screen
        else if (angle < 45)
        {
            yPos = halfScreenHeight - halfImageHeight;
            float normalizedAngle = (angle + 45f) / 90f;
            xPos = Mathf.Lerp(-halfScreenWidth, halfScreenWidth, normalizedAngle);
            float xImageOffset = Mathf.Lerp(halfImageWidth, -halfImageWidth, normalizedAngle);
            xPos += xImageOffset;
        }
        // right side of screen
        else
        {
            xPos = halfScreenWidth - halfImageWidth;
            float normalizedAngle = (angle - 45) / 90f;
            yPos = Mathf.Lerp(halfScreenHeight, -halfScreenHeight, normalizedAngle);
            float yImageOffset = Mathf.Lerp(-halfImageHeight, halfImageHeight, normalizedAngle);
            yPos += yImageOffset;
        }

        arrowRect.anchoredPosition = new Vector3(xPos, yPos, 0);
        // UI rotation is backwards from our system.  Positive angles go counterclockwise
        arrowRect.Rotate(Vector3.forward, -angle);
    }

}
