using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

// Monster 자체의 특징 
// 가지고있는 정보
[RequireComponent(typeof(Rigidbody2D))]
public class Monster : MonoBehaviour, IDamageable
{
    // =============================[ Monster Attributes ]=============================
    [Header("Monster")] 
    [Header("몬스터 데이터")] public MonsterData monsterData;
    [Header("몬스터 체력")] public int HP;   // HP는 각 몬스터들의 공유 데이터x (SO사용안함)
    [Header("점프 힘")]      public float monsterJumpForce     = 5.0f;
    [Header("점프 시간")]    public float jumpTimer            = 0.0f;
    [Header("점프 쿨타임")]   public float monsterJumpDelayTime = 2.0f;


    
    // =============================[ Attack Attributes ]=============================
    [Header("Attack")]
    [Header("공격 관련")]
    [Header("공격 범위")] [SerializeField] private float attackRange = 0.2f;
    [Header("공격 레이어 마스크")][SerializeField] private LayerMask attackTargetMask;
    [Header("점프 레이어 마스크")][SerializeField] private LayerMask obstacleTargetMask; 
    

    private bool canJump = true;        // 점프 가능 여부
    private bool isGrounded = true;     // 바닥 착지중 여부
    private bool isAttacking = false;   // 공격중 여부
    private bool pushBack = false;      // 뒤로 밀림 여부
    
    private Rigidbody2D    rb;
    private Animator       anim;
    private WaitForSeconds jumpCooldownWait;
    private WaitForSeconds backCooldownWait;
    
    private Coroutine      jumpCoroutine = null;
    private Coroutine      backCoroutine = null;
    
    private Collider2D     currentAttackTarget = null;

    // =================================[ Life Cycle ]==================================
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        // Caching
        jumpCooldownWait = new WaitForSeconds(monsterJumpDelayTime);
        backCooldownWait = new WaitForSeconds(0.8f);
    }

    void Start()
    {
        // 수동으로 Mask를 변경하였다.
        // Layer 변경에 따라 수동으로 코드수정 X (코드로 직접 지정)
        attackTargetMask = 1 << LayerMask.NameToLayer("Box"); 
        obstacleTargetMask = 1 << (gameObject.layer);
    }
    
    void FixedUpdate()
    {
        if (isAttacking && !IsAnimationPlaying("Attack")) EndAttack();
        if (isAttacking || pushBack) return;
        
        Vector2 start = (Vector2)transform.position + Vector2.left * 0.7f;
        Vector2 direction = Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(start, direction, attackRange);
        
        // Debug!
        //Debug.DrawLine(start, start + (direction * attackRange), Color.red);
        
        // 앞의 Layer을 감지하여, Truck일 시 공격시도
        // 아닐 시 점프동작
        if (hit)
        {
            // hitLayer : Ray가 감지된 상대 Layer (목표는 truck)
            // 비트연산으로 LayMask 체크
            int hitLayer = hit.collider.gameObject.layer;

            if (DetectInFront(hitLayer, attackTargetMask))
            {
                currentAttackTarget = hit.collider;
                StartAttack();
            }
            else if (DetectInFront(hitLayer, obstacleTargetMask) && isGrounded && canJump) Jump();
            else Move();
        }
        else
        {
            currentAttackTarget = null;
            Move();
        }
    }

    // SetActive(true) - 몬스터 생성 시 정보 Setting
    void OnEnable()
    {
        HP = 100;
        
        canJump = true;
        pushBack = false;
        
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
    
    
    // =================================[ Collider ]==================================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        
        // 밟히면 push back 처리
        if (collision.collider.CompareTag("Monster") && collision.relativeVelocity.y < 0)
        {
            Rigidbody2D otherRb = collision.collider.GetComponent<Rigidbody2D>();
            
            if (!(rb.velocity.y > 0 && otherRb.velocity.y > 0))
            {
                PushBack();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = false;
    }

    
    
    // =================================[ Jump ]==================================
    void Jump()
    {
        rb.velocity = new Vector2(-monsterData.MoveSpeed * 1.2f, monsterJumpForce);
  
        canJump = false;

        if (jumpCoroutine != null)
            StopCoroutine(jumpCoroutine);

        jumpCoroutine = StartCoroutine(JumpDelayTime());
    }

    IEnumerator JumpDelayTime()
    {
        yield return jumpCooldownWait;
        canJump = true;
    }
    
    // =================================[ Attack ]==================================
    void Move()
    {
        if (IsAnimationPlaying("Attack"))
        {
            // 공격 중
           rb.velocity = new Vector2(-(monsterData.MoveSpeed - 2.0f), rb.velocity.y);
        }
        else
        {
            // 공격이 끝남
            rb.velocity = new Vector2(-monsterData.MoveSpeed, rb.velocity.y);
        }
    }
    
    
    // =================================[ Attack ]==================================
    public void StartAttack()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero;
        anim.SetBool("IsAttacking", true);
    }

    public void EndAttack()
    {
        isAttacking = false;
        anim.SetBool("IsAttacking", false);
    }

    // 공격 동작
    public void OnAttack()
    {
        // 때리고 있는 대상에게 데미지 주는 로직
        if (!currentAttackTarget) return;
        
        IDamageable target = currentAttackTarget.GetComponent<IDamageable>();
        Debug.Log(currentAttackTarget.name);
        
        // Box, Player에 대한 데미지처리는 아직 함수 미수현 -> 나중에 처리
        // if (target != null)
        // {
        //     int damage = monsterData.Damage;
        //     target.TakeDamage(damage);
        // }
    }
    
    // 앞에 Layer 감지
    bool DetectInFront(int layer, LayerMask mask)
    {
        return ((1 << layer) & mask) != 0;
    }
    
    // =================================[ Animation ]==================================
    bool IsAnimationPlaying(string animName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(animName) == true)
        {
            // 0 ~ 1 Normalize
            float animTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (animTime < 1.0f)
            {
                return true;
            }
            else if (animTime >= 1.0f)
            {
                return false;
            }
        }

        return false;
    }
     
    //=================================[ Push Back ]==================================   
    void PushBack()
    {
        if (pushBack) return;
    
        pushBack = true;
        float monsterLength = GetComponent<Collider2D>().bounds.size.x;
        Vector2 targetPos = rb.position + new Vector2(monsterLength, 0.0f);
    
        // 중복 실행 방지
        if (backCoroutine != null) StopCoroutine(backCoroutine);
        backCoroutine = StartCoroutine(SmoothMove(targetPos));
    
        // 연쇄 밀림
        Vector2 checkPos = transform.position + Vector3.right * monsterLength;
        Collider2D[] hits = Physics2D.OverlapBoxAll(checkPos, new Vector2(1.0f, 1.0f), 0);
    
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Monster") && hit.gameObject != this.gameObject)
            {
                hit.GetComponent<Monster>()?.PushBack();
                break;
            }
        }
    
        StartCoroutine(ResetPushBack());
    }
    
    IEnumerator SmoothMove(Vector2 target)
    {
        float duration = 0.3f;
        float timer = 0.0f;
        
        Vector2 start = rb.position;
    
        while (timer < duration)
        {
            rb.MovePosition(Vector2.Lerp(start, target, timer / duration));
            timer += Time.deltaTime;
            yield return null;
        }
    
        rb.MovePosition(target);
    }
    
    IEnumerator ResetPushBack()
    {
        yield return backCooldownWait;
        pushBack = false;
    }
    
    //=================================[ Damage Interface ]==================================   
    public void TakeDamage(int damage)
    {
        HP -= damage;
        // 몬스터위에 데미지 출력 나오게 설정
        if (HP <= 0) Dead();
    }
    
    //=================================[ Dead Logic ]==================================    
    public void Dead()
    {
        ObjectPooler.instance.ReturnObject("Monster", gameObject);
        MonsterSpawner.instance.currentMonsters -= 1;
    }
}
