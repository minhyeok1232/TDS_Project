using System.Collections;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [Header("�Ѿ� �̵� �ӵ�")]
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
        // �켱 ���� 1) ���� �浹�� ��Ű��, �������̽��� �Ѱ��ְ� �Ѵ�.
        if (collision.gameObject.CompareTag("Monster"))
        {
            // ����X
            IDamageable target = collision.gameObject.GetComponent<IDamageable>();
            if (target != null)
            {
                // damage ���� 7~25�� �������� �ְ����!
                int randomDamage = Random.Range(7, 26);
                target.TakeDamage(randomDamage);
            }

            ReturnObject();
        }
        
        
        // �켱 ���� 2) Ground�� �浹 ��, Pool ��ȯ
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