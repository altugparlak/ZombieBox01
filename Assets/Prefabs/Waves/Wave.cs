using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]
public class Wave : ScriptableObject
{
    public List<GameObject> zombies;
    public int totalCoin;
    public float coinSpawnPossibility;
    public int totoldualW;
    public float dualWeaponShotSpawnPossibility;
}
