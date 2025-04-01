using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroScript : MonoBehaviour, IDamageable
{
    // =============================[ Monster Attributes ]=============================
    [Header("Box")] 
    [Header("�ִ� ü��")] public int maxHP = 200;
    [Header("���� ü��")] public int currentHp;             // ���� ü��

    [Header("HP �����̴�")]
    public GameObject panel;
    public Slider     slider;      
    
    void Start()
    {
        currentHp = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Destroy();
            currentHp = 0;
        }

        // �����̴� ������Ʈ
        if (panel != null)
        {
            panel.SetActive(true);
            slider.value = currentHp;
        }
        
        // �������� ������ ��� ������ ����
        
    }

    public void Destroy()
    {
        Destroy(gameObject);
        GameManager.Instance.gameState = GameState.GameOver;
    }  
}
