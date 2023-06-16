using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMenuControl : MonoBehaviour
{
    [SerializeField] private Slider dificultySlider;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Mathf.RoundToInt(dificultySlider.value * 100)); //0 ile 100 zorluk
    }
}
