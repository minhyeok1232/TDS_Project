using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Monster 자체의 특징 
// 가지고있는 정보
[RequireComponent(typeof(Rigidbody2D))]
public class Monster : MonoBehaviour
{
    // =============================[ Monster Attributes ]=============================
    [Header("Monster Status")]
    [Header("최대 체력")]  public int   monsterHP            = 100;
    [Header("이동 속도")]  public float monsterSpeed         = 3.0f;
    [Header("점프 힘")]    public float monsterJumpForce     = 100.0f;
    [Header("점프 쿨타임")] public float monsterJumpDelayTime = 1.0f;

    [SerializeField, Tooltip("점프 가능 여부")]
    private bool canJump = true;
    private Rigidbody2D rb;
    private Coroutine jumpCoroutine = null;

    private WaitForSeconds _monsterJumpWaitTime;
    
    // 수정
    private static Dictionary<int, List<GameObject>> blockingMonstersByLayer = new();
    
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Caching
        _monsterJumpWaitTime = new WaitForSeconds(monsterJumpDelayTime);
        

    }
    
    void FixedUpdate()
    {
        if (!canJump) return; // 점프 중이 아닐 때만 이동하도록 조건 추가 가능
        rb.velocity = new Vector2(-monsterSpeed, rb.velocity.y); // 왼쪽으로 이동
    }

    // SetActive(true)
    void OnEnable()
    {
        canJump = true;
        
        if (jumpCoroutine != null)
        {
            StopCoroutine(jumpCoroutine);
            jumpCoroutine = null;
        }
    }
    
    // SetActive(false)
    void OnDisable()
    {
        canJump = false;

        if (jumpCoroutine != null)
        {
            StopCoroutine(jumpCoroutine);
            jumpCoroutine = null;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        int myLayer = gameObject.layer;

        if (!blockingMonstersByLayer.ContainsKey(myLayer))
            blockingMonstersByLayer[myLayer] = new List<GameObject>();
        
        // 트럭과 부딪히면 막힌 리스트 시작
        if (other.CompareTag("Truck"))
        {
            // 1. Layer확인
            // 2. gameObject(내 자신)을 해당 myLayer에 추가
            if (!blockingMonstersByLayer[myLayer].Contains(gameObject))
            {
                blockingMonstersByLayer[myLayer].Add(gameObject);
            }
            return;
        }

        if (IsBlockingChainMonster())
        {
            if (MonsterCheck(other) && canJump)
            {
                Jump();
            }
        }
    }
    
    // 한번더 확인
    bool IsBlockingChainMonster()
    {
        // myLayer : 현재 내 Layer
        int myLayer = gameObject.layer;
        
        // Dictionary의 myLayer값 -> List형식으로 된 막힌 몬스터
        // 내가(gameObject) 포함(Contains)이 되어있나? List형식으로 된 막힌 몬스터 중에서
        return blockingMonstersByLayer.ContainsKey(myLayer) && blockingMonstersByLayer[myLayer].Contains(gameObject);
    }
    
    bool MonsterCheck(GameObject obj)
    {
        return obj.CompareTag("Monster") && obj.layer == gameObject.layer && obj.activeSelf;
    }

    void Jump()
    {
        // JumpDelayTime Coroutine이 실행중이면 금지 (메모리 절약)
        if (jumpCoroutine != null) return;
        
        canJump = false;
        rb.velocity = new Vector2(rb.velocity.x, monsterJumpForce);

        // jumpCoroutine 상태 최신화
        jumpCoroutine = StartCoroutine(JumpDelayTime());
    }

    IEnumerator JumpDelayTime()
    {
        yield return _monsterJumpWaitTime;
        canJump = true;

        // jumpCoroutine 상태 최신화
        jumpCoroutine = null;
    }
}
