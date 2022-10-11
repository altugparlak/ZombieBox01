using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]
public class Wave : ScriptableObject
{
    [Header("Zombies List")]
    [SerializeField] public List<GameObject> zombies;

    [Header("Zombie Spawn Probabilities in the Zombies List")]
    [Tooltip("Probability of the first element in the Zombies List<>")]
    [SerializeField] public int element0;
    [Tooltip("Probability of the second element in the Zombies List<>")]
    [SerializeField] public int element1;
    [Tooltip("Probability of the third element in the Zombies List<>")]
    [SerializeField] public int element2;
    [Tooltip("Probability of the fourth element in the Zombies List<>")]
    [SerializeField] public int element3;
    [Tooltip("Probability of the fifth element in the Zombies List<>")]
    [SerializeField] public int element4;


    [Header("Wave Spects")]
    public int totalNumberOfZombies;
    public int totalCoin;
    public float coinSpawnPossibility;
    public int totoldualW;
    public float dualWeaponShotSpawnPossibility;


}
