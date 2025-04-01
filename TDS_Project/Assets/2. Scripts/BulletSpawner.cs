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
        if (collision.gameObject.CompareTag("Ground"))
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