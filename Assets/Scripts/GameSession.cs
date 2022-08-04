using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] private Text energyText;
    [SerializeField] private Text waveText;

    private int energyAmount;
    private int waveIndex;

    void Start()
    {
        waveIndex = 1; 
        energyAmount = 100;
        energyText.text = energyAmount.ToString();
        waveText.text = $"Wave {waveIndex}";

    }

    public void useEnergy(int amount)
    {
        energyAmount -= amount;
        energyText.text = energyAmount.ToString();

    }


}
