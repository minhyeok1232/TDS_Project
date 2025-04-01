using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Camera camera;

    [Header("ȸ�� ���� ����")]
    public float minAngle = -85.0f;
    public float maxAngle = 0.0f;
    
    [SerializeField] Transform shootStart;
    
    void Start()
    {
        if (camera == null)
            camera = Camera.main;
    }

    void Update()
    {
        // �Ÿ� ���
        float cameraDistance = Mathf.Abs(camera.transform.position.z - transform.position.z);

        // World ��ǥ ��ȯ
        Vector3 mousePos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDistance));

        // ���� ���� ���
        Vector3 direction = mousePos - shootStart.position;

        // ȸ�� ���� ���
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ���� ���� ���
        angle = Mathf.Clamp(angle, minAngle, maxAngle);
        
        // Z�� ���� ȸ�� ����
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}