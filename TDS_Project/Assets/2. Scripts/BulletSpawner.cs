using System.Collections;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [Header("총알 이동 속도")]
    public float speed = 15.0f;

    private Vector3 moveDirection;
    private bool isActive;

    private Coroutine rmCoroutine;

    void OnEnable()
    {
        isActive = true;
        rmCoroutine = StartCoroutine(AutoReturnAfterSeconds(5.0f));
    }

    public void Shoot(Vector2 dir)
    {
        moveDirection = (Vector3)(dir.normalized * speed);
        StartCoroutine(Projectile());
    }

    IEnumerator Projectile()
    {
        while (isActive)
        {
            transform.position += moveDirection * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator AutoReturnAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ReturnObject();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 우선 순위 1) 몬스터 충돌을 시키고, 인터페이스를 넘겨주게 한다.
        if (collision.gameObject.CompareTag("Monster"))
        {
            // 관통X
            IDamageable target = collision.gameObject.GetComponent<IDamageable>();
            if (target != null)
            {
                // damage 값은 7~25로 랜덤값을 주고싶음!
                int randomDamage = Random.Range(7, 26);
                target.TakeDamage(randomDamage);
            }

            ReturnObject();
        }
        
        
        // 우선 순위 2) Ground에 충돌 시, Pool 반환
        if (collision.gameObject.CompareTag("DestroyGround"))
        {
            ReturnObject();
        }
    }

    void ReturnObject()
    {
        if (!isActive) return;
        isActive = false;

        if (rmCoroutine != null)
            StopCoroutine(rmCoroutine);

        ObjectPooler.instance.ReturnObject("Bullet", gameObject);
    }
}