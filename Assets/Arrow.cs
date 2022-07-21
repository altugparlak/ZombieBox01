using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float padding = 1f;
    [SerializeField] float moveSpeed = 5f;
    RectTransform rectTransform;

    PlayerMovement playerMovement;

    float xMin = -600;
    float xMax = 600;

    float yMax = 300;
    float yMin = -300;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        rectTransform.anchoredPosition = new Vector3(-600, 300, 0);
    }

    private void Move()
    {
        float theAngle = playerMovement.GetTheAngleBetweenPlayerAndEnergy();

        var deltaX =  5* Time.deltaTime * moveSpeed;
        var deltaY =  5* Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        rectTransform.anchoredPosition = new Vector2(newXPos, newYPos);

        if (theAngle > 0)
        {
            if (theAngle < 180 && theAngle >= 135)
            {
                rectTransform.anchoredPosition = new Vector2(-600, theAngle);
            }
            else if (theAngle < 135 && theAngle >= 90)
            {
                rectTransform.anchoredPosition = new Vector2(newXPos, 300);
            }
            else if (theAngle < 90 && theAngle >= 45)
            {
                rectTransform.anchoredPosition = new Vector2(newXPos, 300);
            }
            else if (theAngle < 45 && theAngle > 0)
            {
                rectTransform.anchoredPosition = new Vector2(600, newYPos);
            }
        }
        else
        {
            if (theAngle > -180 && theAngle <= -135)
            {
                rectTransform.anchoredPosition = new Vector2(-600, newYPos);
            }
            else if (theAngle > -135 && theAngle <= -90)
            {
                rectTransform.anchoredPosition = new Vector2(newXPos, -300);
            }
            else if (theAngle > -90 && theAngle <= -45)
            {
                rectTransform.anchoredPosition = new Vector2(newXPos, -300);
            }
            else if (theAngle > -45 && theAngle < 0)
            {
                rectTransform.anchoredPosition = new Vector2(600, newYPos);
            }
        }

    }

}
