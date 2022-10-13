using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]
public class Wave : ScriptableObject
{
    [Header("Zombies List")]
    [SerializeField] public List<GameObject> zombies;

    [Header("Zombie Spawn Probabilities in the Zombies List")]
    [Tooltip("1st element is probability of 1st element of the zombies List<>")]
    [SerializeField] public List<int> ZombieSpawnProbs; // Total must be 100


    [Header("Wave Spects")]
    public int totalNumberOfZombies;
    public int totalCoin;
    public float coinSpawnPossibility;
    public int totoldualW;
    public float dualWeaponShotSpawnPossibility;


}
