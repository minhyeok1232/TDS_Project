using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderResize : MonoBehaviour
{
    [SerializeField] GameObject box;
    private BoxCollider2D col;

    private float boxWidth;
    private float boxHeight;
    
    void Awake()
    {
        boxWidth = box.GetComponent<BoxCollider2D>().size.x;
        boxHeight = box.GetComponent<BoxCollider2D>().size.y;
        
        col = GetComponent<BoxCollider2D>();
        if (col == null) col = gameObject.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
    }

    void Start()
    {
        // 맨 처음도 박스크기 Setting
        ColliderReSize(transform.childCount - 1);
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

    public void ColliderReSize(int cnt)
    {
        float totalHeight = 0.0f;

        for (int i = 0; i < cnt; i++)
        {
            Transform child = transform.GetChild(i);
            BoxCollider2D boxCol = child.GetComponent<BoxCollider2D>();
            if (boxCol != null)
            {
                totalHeight += boxCol.size.y;
            }
        }

        col.size = new Vector2(boxWidth, totalHeight);
        col.offset = new Vector2(0f, totalHeight / 2f);
    }
}