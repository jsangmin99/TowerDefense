using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeUnit : MonoBehaviour
{
    public float detectionRange = 10f; // 적 감지 범위
    public int attackDamage = 10;
    public float defense = 5f;
    public float health = 100f;
    public float attackSpeed = 1f; // 공격 속도 (공격 간격)

    private bool isAttacking = false;
    private Enemy currentEnemy;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // 물리 시뮬레이션에 영향을 받지 않도록 Rigidbody를 kinematic으로 설정
    }

    void Update()
    {
        if (!isAttacking)
        {
            FindEnemyToAttack();
        }
    }

    void FindEnemyToAttack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    currentEnemy = enemy;
                    StartCoroutine(AttackEnemy());
                    break;
                }
            }
        }
    }

    IEnumerator AttackEnemy()
    {
        isAttacking = true;
        Debug.Log("MeleeUnit: Attacking");

        // 이동을 멈추고 적을 향해 회전
        Vector3 direction = currentEnemy.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;

        while (currentEnemy != null && currentEnemy.health > 0 && health > 0)
        {
            // 적을 공격
            currentEnemy.TakeDamage(attackDamage);

            yield return new WaitForSeconds(attackSpeed);
        }

        isAttacking = false;
        Debug.Log("MeleeUnit: Attack finished");
        currentEnemy = null; // 현재 공격 중인 적 해제
    }

    public void TakeDamage(float damage)
    {
        health -= damage * 100 / (100 + defense);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (currentEnemy != null)
        {
            currentEnemy.StopAttacking(); // 공격 중인 적이 있다면 공격 중단
        }

        // 아군 근접 유닛이 사망할 때의 동작 구현
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Melee")) // 상대방 태그 확인
        {
            Debug.Log("MeleeUnit: OnTriggerEnter");
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && !isAttacking)
            {
                currentEnemy = enemy;
                StartCoroutine(AttackEnemy());
            }
        }
    }

    public void StopAttacking()
    {
        StopAllCoroutines(); // 모든 공격 코루틴 중지
        isAttacking = false;
        currentEnemy = null;
    }
}
