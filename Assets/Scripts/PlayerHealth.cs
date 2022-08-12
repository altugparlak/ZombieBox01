using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private Slider healthbar;

    private int playerHealth;
    void Start()
    {
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
    }

    public void addHealth(int value0)
    {
        playerHealth += value0;
        healthbar.value = ((float)playerHealth / (float)1000);
    }
}
