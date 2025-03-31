using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

// Monster ��ü�� Ư¡ 
// �������ִ� ����
[RequireComponent(typeof(Rigidbody2D))]
public class Monster : MonoBehaviour
{
    // =============================[ Monster Attributes ]=============================
    [Header("Monster")]
    [Header("�ִ� ü��")]  public int   monsterHP            = 100;
    [Header("�̵� �ӵ�")]  public float monsterSpeed         = 3.0f;
    [Header("���� ��")]    public float monsterJumpForce     = 5.0f;
    [Header("���� �ð�")]  public float jumpTimer            = 0.0f;
    [Header("���� ��Ÿ��")] public float monsterJumpDelayTime = 2.0f;


    
    // =============================[ Attack Attributes ]=============================
    [Header("Attack")]
    [Header("���� ����")]
    [Header("���� ����")] [SerializeField] private float attackRange = 0.2f;
    [Header("���� ���̾� ����ũ")][SerializeField] private LayerMask attackTargetMask;
    [Header("���� ���̾� ����ũ")][SerializeField] private LayerMask obstacleTargetMask; 
    

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
        // �������� Mask�� �����Ͽ���.
        // Layer ���濡 ���� �������� �ڵ���� X (�ڵ�� ���� ����)
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
        
        // ���� Layer�� �����Ͽ�, Truck�� �� ���ݽõ�
        // �ƴ� �� ��������
        if (!isAttacking)
        {
            if (hit)
            {
                // hitLayer : Ray�� ������ ��� Layer (��ǥ�� truck)
                // ��Ʈ�������� LayMask üũ
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
            // ���� ��
            rb.velocity = new Vector2(-(monsterSpeed - 2.0f), rb.velocity.y);
        }
        else
        {
            // ������ ����
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

    // ���� ����
    public void Attack()
    {
        
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
}
