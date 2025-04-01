using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Camera camera;

    [Header("회전 제한 범위")]
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
        // 거리 계산
        float cameraDistance = Mathf.Abs(camera.transform.position.z - transform.position.z);

        // World 좌표 변환
        Vector3 mousePos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDistance));

        // 방향 벡터 계산
        Vector3 direction = mousePos - shootStart.position;

        // 회전 각도 계산
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 각도 범위 계산
        angle = Mathf.Clamp(angle, minAngle, maxAngle);
        
        // Z축 기준 회전 적용
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}