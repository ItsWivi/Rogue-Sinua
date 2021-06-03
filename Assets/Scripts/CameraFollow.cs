using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetObject;

    public Vector3 cameraOffset;

    public float smoothFactor = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //The offset maintained will be the same as the camera has when Play is hit
        cameraOffset = transform.position - targetObject.transform.position;
    }

    // Update is called once per frame, LateUpdate is called right after Update. We do this so that the always Player moves first. 
    // The smoothFactor is then neccessary to control jitteriness
    void LateUpdate()
    {
        Vector3 newPosition = targetObject.transform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);
    }
}
