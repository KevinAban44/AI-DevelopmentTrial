using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] Camera myCamera;
    public Vector3 offset;


    public void LateUpdate()
    {
        myCamera.transform.position = target.position + offset;
        myCamera.transform.LookAt(target);
    }
}
