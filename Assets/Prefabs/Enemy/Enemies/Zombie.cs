using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Zombie", menuName = "Zombie")]
public class Zombie : ScriptableObject
{
    [Tooltip("Zombie Difficulty Coefficient, must be between 0 and 1")]
    [SerializeField] public float zdc; // This value provides how many times you wanna see this zombie on a level
    [SerializeField] public AudioClip zombieAttackSound;
    [SerializeField] public AudioClip zombieDeathSound;
    public new string name;
    public int health;
    public float movementSpeed;
    public float attackSpeed;
    public int attackDamage;
    public bool smartAttack;
}
