using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBrain : MonoBehaviour
{
    [SerializeField] private Text remainingSeconds;
    [SerializeField] private Image transparentImage;
    [SerializeField] private GameObject loadingRedImage0;
    [SerializeField] private GameObject loadingRedImage1;
    [SerializeField] private float rotateSpeed = 1f;
    [SerializeField] private float timeRemaining = 3.5f;
    Button button;
    RectTransform rt;
    GameSession gameSession;
    private bool buttonClicked = false;

    private float firstwidthValue;
    private float buttoncooldown;
    private float timeRemHolder;

    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        rt = transparentImage.GetComponent<RectTransform>();
        button = GetComponent<Button>();

        button.enabled = true;
        remainingSeconds.enabled = false;
        transparentImage.enabled = false;
        timeRemHolder = timeRemaining;
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
            ButtonLoading();
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
            this.gameObject.GetComponent<Image>().enabled = true;
            loadingRedImage0.SetActive(false);
            loadingRedImage1.SetActive(false);
            remainingSeconds.enabled = false;
            transparentImage.enabled = false;
            button.enabled = true;
            buttonClicked = false;
            timeRemaining = timeRemHolder;

        }

    }

    public void ButtonClickControl()
    {
        if (gameSession.energyUsable)
        {
            rt.sizeDelta = new Vector2(firstwidthValue, firstwidthValue);
            buttonClicked = true;
        }

    }

    private void ButtonLoading()
    {
        this.gameObject.GetComponent<Image>().enabled = false;
        loadingRedImage0.SetActive(true);
        loadingRedImage1.SetActive(true);
        loadingRedImage0.transform.Rotate(0, 0, -6.0f * rotateSpeed * Time.deltaTime);
        loadingRedImage1.transform.Rotate(0, 0, 6.0f * rotateSpeed * Time.deltaTime);
    }
}
