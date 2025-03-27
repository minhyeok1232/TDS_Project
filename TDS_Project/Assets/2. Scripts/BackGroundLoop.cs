using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 배경 재배치
public class BackGroundLoop : MonoBehaviour 
{
    [SerializeField] 
    private Transform backGroundImage; 

    private float width;
    
    void Awake() 
    {
        width = backGroundImage.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Start()
    {
        if (backGroundImage == null)
        {
            Helpers.LogError("backGroundImage is Null");
            return;
        }
    }
    
    private void Update() 
    {
        transform.Translate(2.0f * Time.deltaTime * Vector2.left);
        if (transform.position.x < -width)
        {
            Reposition();
        }
    }

    void Reposition() 
    {
        Vector2 offset = new Vector2(width * 2, 0);
        transform.position = (Vector2)transform.position + offset;
    }
}