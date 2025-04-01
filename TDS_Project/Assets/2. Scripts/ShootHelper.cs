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
    [Header("시작점")] public Transform  shootStart;
    [Header("플래시")] public GameObject muzzleFlash;
    [Header("총알")]   public int   bulletCount;
    [Header("산탄")]   public float angle;
    [Header("시간")]   public float timer;
    [Header("속도")]   public float attackSpeed;
    
    // ===============================[ Bullet Attributes ]===============================
    [Header("시각 총알")]
    public GameObject bulletPrefab;
    
    [Header("RayCast")]
    public LayerMask targetMask;
    public float range = 10.0f;            // 최대 사거리
    public float damage = 10.0f;           // 기본 데미지

    private WaitForSeconds wait;
    
    [Header("회전 기준")]
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

        // 총알 개수만큼 Raycast
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
    
    // 총구 플래시 재생
    IEnumerator Flash()
    {
        muzzleFlash.SetActive(true);
        yield return wait;
        muzzleFlash.SetActive(false);
    }
}
