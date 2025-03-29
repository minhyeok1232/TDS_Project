using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Monster ��ü�� Ư¡ 
// �������ִ� ����
[RequireComponent(typeof(Rigidbody2D))]
public class Monster : MonoBehaviour
{
    // =============================[ Monster Attributes ]=============================
    [Header("Monster Status")]
    [Header("�ִ� ü��")]  public int   monsterHP            = 100;
    [Header("�̵� �ӵ�")]  public float monsterSpeed         = 3.0f;
    [Header("���� ��")]    public float monsterJumpForce     = 100.0f;
    [Header("���� ��Ÿ��")] public float monsterJumpDelayTime = 1.0f;

    [SerializeField, Tooltip("���� ���� ����")]
    private bool canJump = true;
    private Rigidbody2D rb;
    private Coroutine jumpCoroutine = null;

    private WaitForSeconds _monsterJumpWaitTime;
    
    // ����
    private static Dictionary<int, List<GameObject>> blockingMonstersByLayer = new();
    
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Caching
        _monsterJumpWaitTime = new WaitForSeconds(monsterJumpDelayTime);
        

    }
    
    void FixedUpdate()
    {
        if (!canJump) return; // ���� ���� �ƴ� ���� �̵��ϵ��� ���� �߰� ����
        rb.velocity = new Vector2(-monsterSpeed, rb.velocity.y); // �������� �̵�
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
        
        // Ʈ���� �ε����� ���� ����Ʈ ����
        if (other.CompareTag("Truck"))
        {
            // 1. LayerȮ��
            // 2. gameObject(�� �ڽ�)�� �ش� myLayer�� �߰�
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
    
    // �ѹ��� Ȯ��
    bool IsBlockingChainMonster()
    {
        // myLayer : ���� �� Layer
        int myLayer = gameObject.layer;
        
        // Dictionary�� myLayer�� -> List�������� �� ���� ����
        // ����(gameObject) ����(Contains)�� �Ǿ��ֳ�? List�������� �� ���� ���� �߿���
        return blockingMonstersByLayer.ContainsKey(myLayer) && blockingMonstersByLayer[myLayer].Contains(gameObject);
    }
    
    bool MonsterCheck(GameObject obj)
    {
        return obj.CompareTag("Monster") && obj.layer == gameObject.layer && obj.activeSelf;
    }

    void Jump()
    {
        // JumpDelayTime Coroutine�� �������̸� ���� (�޸� ����)
        if (jumpCoroutine != null) return;
        
        canJump = false;
        rb.velocity = new Vector2(rb.velocity.x, monsterJumpForce);

        // jumpCoroutine ���� �ֽ�ȭ
        jumpCoroutine = StartCoroutine(JumpDelayTime());
    }

    IEnumerator JumpDelayTime()
    {
        yield return _monsterJumpWaitTime;
        canJump = true;

        // jumpCoroutine ���� �ֽ�ȭ
        jumpCoroutine = null;
    }
}
