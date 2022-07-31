using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] private Text energy;

    private int energyAmount;

    void Start()
    {
        energyAmount = 100;
        energy.text = energyAmount.ToString();
    }

    public void useEnergy(int amount)
    {
        energyAmount -= amount;
        energy.text = energyAmount.ToString();

    }


}
