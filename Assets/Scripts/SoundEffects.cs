using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound0;
    [SerializeField] private AudioClip clickSound1;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClickSound0()
    {
        audioSource.PlayOneShot(clickSound0);
    }

    public void PlayClickSound1()
    {
        audioSource.PlayOneShot(clickSound1);
    }
}
