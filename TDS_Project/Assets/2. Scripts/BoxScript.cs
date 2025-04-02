using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxScript : MonoBehaviour, IDamageable
{
    // =============================[ Monster Attributes ]=============================
    [Header("Box")] 
    [Header("최대 체력")] public int maxHP = 100;
    [Header("현재 체력")] public int currentHp;             // 현재 체력

    [Header("HP 슬라이더")]
    public GameObject panel;
    public Slider     slider;      
    
    void Start()
    {
        currentHp = maxHP;
    }
    
    void Update()
    {
        
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        // Ignore Collision
        if (other.gameObject.CompareTag("Ground"))
        {
            if (other.collider && GetComponent<Collider2D>())
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(),  other.collider);
        }
    }
    
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Destroy();
            currentHp = 0;
        }

        // 슬라이더 업데이트
        if (panel != null)
        {
            panel.SetActive(true);
            slider.value = currentHp;
        }
        
        // 몬스터위에 데미지 출력 나오게 설정
        
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
