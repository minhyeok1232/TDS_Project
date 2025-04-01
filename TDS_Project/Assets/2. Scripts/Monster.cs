using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

// Monster ��ü�� Ư¡ 
// �������ִ� ����
[RequireComponent(typeof(Rigidbody2D))]
public class Monster : MonoBehaviour, IDamageable
{
    // =============================[ Monster Attributes ]=============================
    [Header("Monster")] 
    [Header("���� ������")] public MonsterData monsterData;
    [Header("���� ü��")] public int HP;   // HP�� �� ���͵��� ���� ������x (SO������)
    [Header("���� ��")]      public float monsterJumpForce     = 5.0f;
    [Header("���� �ð�")]    public float jumpTimer            = 0.0f;
    [Header("���� ��Ÿ��")]   public float monsterJumpDelayTime = 2.0f;


    
    // =============================[ Attack Attributes ]=============================
    [Header("Attack")]
    [Header("���� ����")]
    [Header("���� ����")] [SerializeField] private float attackRange = 0.2f;
    [Header("���� ���̾� ����ũ")][SerializeField] private LayerMask attackTargetMask;
    [Header("���� ���̾� ����ũ")][SerializeField] private LayerMask obstacleTargetMask; 
    

    private bool canJump = true;        // ���� ���� ����
    private bool isGrounded = true;     // �ٴ� ������ ����
    private bool isAttacking = false;   // ������ ����
    private bool pushBack = false;      // �ڷ� �и� ����
    
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
        // �������� Mask�� �����Ͽ���.
        // Layer ���濡 ���� �������� �ڵ���� X (�ڵ�� ���� ����)
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
        
        // ���� Layer�� �����Ͽ�, Truck�� �� ���ݽõ�
        // �ƴ� �� ��������
        if (hit)
        {
            // hitLayer : Ray�� ������ ��� Layer (��ǥ�� truck)
            // ��Ʈ�������� LayMask üũ
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

    // SetActive(true) - ���� ���� �� ���� Setting
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
        
        // ������ push back ó��
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
            // ���� ��
           rb.velocity = new Vector2(-(monsterData.MoveSpeed - 2.0f), rb.velocity.y);
        }
        else
        {
            // ������ ����
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

    // ���� ����
    public void OnAttack()
    {
        // ������ �ִ� ��󿡰� ������ �ִ� ����
        if (!currentAttackTarget) return;
        
        IDamageable target = currentAttackTarget.GetComponent<IDamageable>();
        Debug.Log(currentAttackTarget.name);
        
        // Box, Player�� ���� ������ó���� ���� �Լ� �̼��� -> ���߿� ó��
        // if (target != null)
        // {
        //     int damage = monsterData.Damage;
        //     target.TakeDamage(damage);
        // }
    }
    
    // �տ� Layer ����
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
    
        // �ߺ� ���� ����
        if (backCoroutine != null) StopCoroutine(backCoroutine);
        backCoroutine = StartCoroutine(SmoothMove(targetPos));
    
        // ���� �и�
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
        // �������� ������ ��� ������ ����
        if (HP <= 0) Dead();
    }
    
    //=================================[ Dead Logic ]==================================    
    public void Dead()
    {
        ObjectPooler.instance.ReturnObject("Monster", gameObject);
        MonsterSpawner.instance.currentMonsters -= 1;
    }
}
