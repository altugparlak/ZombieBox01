using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class targetPointer : MonoBehaviour
{

    public Transform targetPosition;
    private RectTransform pointerRectTransform;

    public Camera cameraPlayer;



    private void Awake()
    {
        
        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();

    }
   

    
    void Update()
    {
        Vector3 toPosition = targetPosition.position;
        Vector3 fromPosition = cameraPlayer.transform.position;

        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        Debug.Log(dir);
        float angle = UtilsClass.GetAngleFromVectorFloat(dir);
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
