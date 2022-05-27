using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera _camera;
    void LateUpdate()
    {
        
        if (_camera == null)
        {
            _camera = Camera.main;
            return;
        }
        // Face the Camera
        transform.rotation = _camera.transform.rotation;
    }
}
