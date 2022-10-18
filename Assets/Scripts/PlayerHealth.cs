using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Slider healthbar;

    GameSession gameSession;

    private int playerHealth;
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();

        playerHealth = 1000;
        healthbar.value = ((float)playerHealth / (float)1000);
        
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    public void losePlayerHealth(int value)
    {
        playerHealth -= value;
        healthbar.value = ((float)playerHealth / (float)1000);
        if (playerHealth <= 0)
        {
            Debug.Log("You died!");
            this.gameObject.GetComponent<PlayerMovement>().notDeath = false;
            this.gameObject.GetComponent<PlayerShoot>().notDeath1 = false;
            gameSession.ActivateGameEndWindow();
            //Destroy(this.gameObject);
        }
    }

    public void addHealth(int value0)
    {
        playerHealth += value0;
        healthbar.value = ((float)playerHealth / (float)1000);
    }
}
