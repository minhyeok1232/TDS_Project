using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

// Monster 자체의 특징 
// 가지고있는 정보
[RequireComponent(typeof(Rigidbody2D))]
public class Monster : MonoBehaviour
{
    // =============================[ Monster Attributes ]=============================
    [Header("Monster")]
    [Header("최대 체력")]  public int   monsterHP            = 100;
    [Header("이동 속도")]  public float monsterSpeed         = 3.0f;
    [Header("점프 힘")]    public float monsterJumpForce     = 5.0f;
    [Header("점프 시간")]  public float jumpTimer            = 0.0f;
    [Header("점프 쿨타임")] public float monsterJumpDelayTime = 2.0f;


    
    // =============================[ Attack Attributes ]=============================
    [Header("Attack")]
    [Header("공격 관련")]
    [Header("공격 범위")] [SerializeField] private float attackRange = 0.2f;
    [Header("공격 레이어 마스크")][SerializeField] private LayerMask attackTargetMask;
    [Header("점프 레이어 마스크")][SerializeField] private LayerMask obstacleTargetMask; 
    

    private bool canJump = true;
    private bool isGrounded = true;
    private bool isAttacking = false;
    
    private Rigidbody2D    rb;
    private Animator       anim;
    private Coroutine      jumpCooldownCoroutine = null;
    private WaitForSeconds jumpCooldownWait;

    // =================================[ Life Cycle ]==================================
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        // Caching
        jumpCooldownWait = new WaitForSeconds(monsterJumpDelayTime);
    }

    void Start()
    {
        // 수동으로 Mask를 변경하였다.
        // Layer 변경에 따라 수동으로 코드수정 X (코드로 직접 지정)
        attackTargetMask = 1 << LayerMask.NameToLayer("Truck"); 
        obstacleTargetMask = 1 << (gameObject.layer);
    }
    
    void FixedUpdate()
    {
        Vector2 start = (Vector2)transform.position + Vector2.left * 0.7f;
        Vector2 direction = Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(start, direction, attackRange);
        
        // Debug!
        Debug.DrawLine(start, start + (direction * monsterSpeed), Color.red, attackRange);
        
        // 앞의 Layer을 감지하여, Truck일 시 공격시도
        // 아닐 시 점프동작
        if (!isAttacking)
        {
            if (hit)
            {
                // hitLayer : Ray가 감지된 상대 Layer (목표는 truck)
                // 비트연산으로 LayMask 체크
                int hitLayer = hit.collider.gameObject.layer;
                
                if (DetectInFront(hitLayer, attackTargetMask)) StartAttack();
                else if (DetectInFront(hitLayer, obstacleTargetMask) && isGrounded && canJump) Jump();
                else Move();
            }
            else
            {
                Move();
            }
        }
    }

    // SetActive(true)
    void OnEnable()
    {
        canJump = true;
        
        if (jumpCooldownCoroutine != null)
        {
            StopCoroutine(jumpCooldownCoroutine);
            jumpCooldownCoroutine = null;
        }
    }
    
    // SetActive(false)
    void OnDisable()
    {
        canJump = false;

        if (jumpCooldownCoroutine != null)
        {
            StopCoroutine(jumpCooldownCoroutine);
            jumpCooldownCoroutine = null;
        }
    }
    
    
    // =================================[ Collider ]==================================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
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
        rb.velocity = new Vector2(rb.velocity.x, monsterJumpForce);
        canJump = false;

        if (jumpCooldownCoroutine != null)
            StopCoroutine(jumpCooldownCoroutine);

        jumpCooldownCoroutine = StartCoroutine(JumpDelayTime());
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
            rb.velocity = new Vector2(-(monsterSpeed - 2.0f), rb.velocity.y);
        }
        else
        {
            // 공격이 끝남
            rb.velocity = new Vector2(-monsterSpeed, rb.velocity.y);
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
    public void Attack()
    {
        
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
}
