using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f; // 화살의 이동 속도
    public int damage; // 화살의 공격력

    private Transform target; // 화살의 타겟

    public void SetTarget(Transform target, int damage)
    {
        this.target = target;
        this.damage = damage;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // 타겟이 없어지면 화살 제거
            return;
        }

        // 타겟 방향으로 이동
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        // 타겟에게 공격을 가하고 화살 제거
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null) 
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
