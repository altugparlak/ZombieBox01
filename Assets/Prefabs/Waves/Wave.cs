using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]
public class Wave : ScriptableObject
{
    [Header("Zombies List")]
    [SerializeField] public List<GameObject> zombies;

    [Header("Wave Spects")]
    public int totalNumberOfZombies;

}
