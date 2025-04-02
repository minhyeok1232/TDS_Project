using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageScript : MonoBehaviour
{
    [Header("TMP")]
    public TextMeshProUGUI damageText;

    [Header("Attribute")]
    private float floatSpeed;
    private float fadeTime;
    private float floatDistance;
    private float timer;
    private bool  isPlaying;
    private Color startColor;
    
    [Header("Position")]
    private Vector3 startPosition;
    private Vector3 endPosition;

    void OnEnable()
    {
        floatSpeed = 0.5f;
        fadeTime = 0.5f;
        floatDistance = 1.0f;
        timer = 0.0f;

        startColor = Color.white;
        
        isPlaying = false;
    }

    public void PrintDamage(int amount, Vector3 spawnPosition)
    {
        // Critical ȿ��
        damageText.color = amount >= 18 ? Color.red : Color.white;
        startColor = damageText.color; // ? �̰� �ݵ�� �־���� ��!
        
        damageText.text = amount.ToString();
        // ���⼭ ���� ���ݾ� �̵��ϸ鼭 ���� ���������鼭 0.5�������� ReturnToPool
        
        startPosition = spawnPosition;
        endPosition   = startPosition + new Vector3(0.0f, 0.5f, 0.0f);
        transform.position = startPosition;
        
        timer = 0.0f;
        isPlaying = true;
    }
    
    void Update()
    {
        if (!isPlaying) return;

        timer += Time.deltaTime;
        float t = timer / fadeTime;
        
        // ���� �̵�
        transform.position = Vector3.Lerp(startPosition, endPosition, t);

        // ���İ� ����
        Color c = startColor;
        c.a = Mathf.Lerp(1f, 0f, t);
        damageText.color = c;

        // Return Pool
        if (timer >= fadeTime)
        {
            isPlaying = false;
            damageText.color = startColor; 
            ReturnToPool();
        }
    }

    public void ReturnToPool()
    {
        ObjectPooler.instance.ReturnObject("Damage",gameObject);
    }
}
