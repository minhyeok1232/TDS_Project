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
        // Critical 효과
        damageText.color = amount >= 18 ? Color.red : Color.white;
        startColor = damageText.color; // ? 이거 반드시 넣어줘야 함!
        
        damageText.text = amount.ToString();
        // 여기서 위로 조금씩 이동하면서 점점 투명해지면서 0.5초지나면 ReturnToPool
        
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
        
        // 보간 이동
        transform.position = Vector3.Lerp(startPosition, endPosition, t);

        // 알파값 조절
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
