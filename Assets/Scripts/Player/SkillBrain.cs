using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBrain : MonoBehaviour
{
    //ShockWaveBrain
    [SerializeField] private AnimationCurve curve;
    SphereCollider thisCollider;
    private float colliderRadius = 0f;
    private float speed = 0f;
    private bool startGrowing = false;
    void Start()
    {
        thisCollider = GetComponent<SphereCollider>();
        Invoke("SelfDestruction", 0.75f);
        Invoke("StartGrow", 0.15f);
        //Time.timeScale = 0.1f;
    }

    void Update()
    {
        if (startGrowing)
        {
            speed = Mathf.Lerp(speed, 8, Time.deltaTime * 0.5f);
            colliderRadius = Mathf.Lerp(colliderRadius, 7.5f, Time.deltaTime * speed);
            thisCollider.radius = colliderRadius;
        }

    }

    private void SelfDestruction()
    {
        Destroy(this.gameObject);
    }

    private void StartGrow()
    {
        startGrowing = true;
    }

}
