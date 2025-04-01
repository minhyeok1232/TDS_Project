using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootHelper : MonoBehaviour
{
    // ===============================[ Hero Attributes ]==============================
    [Header("Hero")]
    public Animator anim;
    
    // ===============================[ Gun Attributes ]===============================
    [Header("Gun")] 
    [Header("������")] public Transform  shootStart;
    [Header("�÷���")] public GameObject muzzleFlash;
    [Header("�Ѿ�")]   public int   bulletCount;
    [Header("��ź")]   public float angle;
    [Header("�ð�")]   public float timer;
    [Header("�ӵ�")]   public float attackSpeed;
    
    // ===============================[ Bullet Attributes ]===============================
    [Header("�ð� �Ѿ�")]
    public GameObject bulletPrefab;
    
    [Header("RayCast")]
    public LayerMask targetMask;
    public float range = 10.0f;            // �ִ� ��Ÿ�
    public float damage = 10.0f;           // �⺻ ������

    private WaitForSeconds wait;
    
    [Header("ȸ�� ����")]
    public Transform rotator;
    
    // =================================[ Life Cycle ]==================================
    void Awake()
    {
        bulletCount = 5;
        attackSpeed = 1.0f;
        angle = 10.0f;
        wait = new WaitForSeconds(0.5f);
    }
    
    void Update()
    {
        anim.SetFloat("AnimSpeed", 1 / attackSpeed);
        timer += Time.deltaTime;
        if (timer >= attackSpeed)
        {
            timer = 0;
            Shoot();
        }
    }
    
    
    // =================================[ Shoot ]==================================
    public void Shoot()
    {
        StartCoroutine(Flash());
        anim.SetTrigger("Shoot");
        
        // rotator : Gun Object
        Quaternion baseRotation = rotator.rotation;

        // �Ѿ� ������ŭ Raycast
        for (int i = 0; i < bulletCount; i++)
        {
            float spreadOffset = Random.Range(-angle, angle);
            Quaternion spreadRotation = baseRotation * Quaternion.Euler(0, 0, spreadOffset);
            
            Vector2 shootDirection = spreadRotation * new Vector2(0.5f, 0.28f);
            
            // Object Pool
            GameObject bullet = ObjectPooler.instance.GetObject("Bullet");
            bullet.transform.position = shootStart.position;
            bullet.transform.rotation = spreadRotation;
            bullet.GetComponent<BulletSpawner>().Shoot(shootDirection);
        }
    }
    
    // �ѱ� �÷��� ���
    IEnumerator Flash()
    {
        muzzleFlash.SetActive(true);
        yield return wait;
        muzzleFlash.SetActive(false);
    }
}
