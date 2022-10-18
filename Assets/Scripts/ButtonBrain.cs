using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBrain : MonoBehaviour
{
    [SerializeField] private Text remainingSeconds;
    [SerializeField] private Image transparentImage;
    [SerializeField] private float timeRemaining = 3.5f;
    Button button;
    RectTransform rt;
    private bool buttonClicked = false;

    private float firstwidthValue;
    private float buttoncooldown;

    void Start()
    {
        rt = transparentImage.GetComponent<RectTransform>();
        button = GetComponent<Button>();

        button.enabled = true;
        remainingSeconds.enabled = false;
        transparentImage.enabled = false;

        buttoncooldown = timeRemaining;
        firstwidthValue = rt.sizeDelta.x;
    }

    void Update()
    {
        if (buttonClicked)
        {
            transparentImage.enabled = true;
            remainingSeconds.enabled = true;
            slowlyDissapearTransparent();
            ButtonCooldownCountdown();
        }
    }

    public void slowlyDissapearTransparent()
    {
        float widthValue = rt.sizeDelta.x;
        float newWidth = Mathf.MoveTowards(widthValue, 0, (firstwidthValue/buttoncooldown) * Time.deltaTime); // width'i cooldown süresine böldük 80/3
        rt.sizeDelta = new Vector2(newWidth, newWidth);
    }

    private void ButtonCooldownCountdown()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            remainingSeconds.text = timeRemaining.ToString();
            button.enabled = false;
        }
        else
        {
            timeRemaining = 0;
            remainingSeconds.enabled = false;
            transparentImage.enabled = false;
            button.enabled = true;
            buttonClicked = false;
        }

    }

    public void ButtonClickControl()
    {
        rt.sizeDelta = new Vector2(firstwidthValue, firstwidthValue);
        buttonClicked = true;
    }
}
