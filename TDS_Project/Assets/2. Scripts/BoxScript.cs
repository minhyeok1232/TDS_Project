using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxScript : MonoBehaviour, IDamageable
{
    // =============================[ Monster Attributes ]=============================
    [Header("Box")] 
    [Header("�ִ� ü��")] public int maxHP = 100;
    [Header("���� ü��")] public int currentHp;             // ���� ü��

    [Header("HP �����̴�")]
    public GameObject panel;
    public Slider     slider;      
    
    void Start()
    {
        currentHp = maxHP;
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

        if (panel != null && slider != null)
        {
            panel.SetActive(true);
            slider.value = (float)currentHp / maxHP;
        }
    }

    public void Destroy()
    {
        BoxColliderResize resize = transform.parent.GetComponent<BoxColliderResize>();
        if (resize)
        {
            resize.ColliderReSize(transform.parent.childCount - 2);
        }
        Destroy(gameObject);
    }
}
