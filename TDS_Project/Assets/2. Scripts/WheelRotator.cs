using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotator : MonoBehaviour
{
    [SerializeField]
    private Transform wheels;
    
    private float rotateSpeed = 360f; 
    void Update()
    {
        transform.Rotate(Vector3.forward, -rotateSpeed * Time.deltaTime);
    }
}
