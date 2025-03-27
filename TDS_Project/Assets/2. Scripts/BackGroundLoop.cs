using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 왼쪽 끝으로 이동한 배경을 오른쪽 끝으로 재배치하는 스크립트
public class BackGroundLoop : MonoBehaviour {
    
    [SerializeField] 
    private Transform BackGroundBox; 

    private float width;
    
    void Awake() 
    {
        width = BackGroundBox.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Start()
    {
        if (BackGroundBox == null)
        {
            Debug.LogError("BackGroundBox : X");
            return;
        }
    }
    
    private void Update() {
        // 현재 위치가 원점에서 왼쪽으로 width 이상 이동했을때 위치를 리셋
    }

    // 위치를 리셋하는 메서드
    void Reposition() 
    {
        
    }
}