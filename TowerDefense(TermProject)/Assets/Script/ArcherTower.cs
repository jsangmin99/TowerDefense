using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : MonoBehaviour
{
    public float detectionRange; // 궁수 타워의 적 인식 범위
    public int maxHealth; // 궁수 타워의 최대 체력
    public float attackRange; // 궁수 타워의 공격 사거리
    public Transform attackRangeIndicator; // 스프라이트 또는 UI 요소의 Transform
    public float attackSpeed; // 궁수 타워의 공격 속도
    public int attackDamage; // 궁수 타워의 공격력


    public GameObject arrowPrefab; // 궁수 타워의 화살 프리팹
    public Transform firePoint; // 화살 발사 위치

    private Transform target; // 현재 타겟으로 지정된 적 유닛

    private float attackTimer; // 공격 타이머

    void Start()
    {
        attackTimer = 0f;
    }

    void Update()
    {
        // 타겟 적 유닛이 존재하고 사거리 내에 있는지 확인
        if (target != null && Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            // 일정 간격으로 공격 수행
            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = 1f / attackSpeed; // 공격 간격 계산
            }
        }
        else
        {
            // 타겟이 없거나 사거리를 벗어난 경우 타겟 새로 설정
            target = FindTarget();
        }
        // 공격 타이머 감소
        attackTimer -= Time.deltaTime;
        // 공격 범위 표시 업데이트
        UpdateAttackRangeIndicator();
    }

    void Attack()
    {
        Debug.Log("Attack() callde");
        // 화살 프리팹을 생성하여 타겟 방향으로 발사
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        Arrow arrowComponent = arrow.GetComponent<Arrow>();
        if (arrowComponent != null)
        {
            arrowComponent.SetTarget(target, attackDamage);
        }
    }

    Transform FindTarget()
    {

        // 인식 범위 내에서 가장 가까운 타겟 적 유닛 탐색
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Debug.Log("Find() callde");

                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {

                    closestDistance = distance;
                    closestTarget = collider.transform;
                }
            }
        }

        return closestTarget;
    }
    private void UpdateAttackRangeIndicator()
    {
        // 공격 범위 표시 업데이트
        if (attackRangeIndicator != null)
        {
            attackRangeIndicator.localScale = new Vector3(attackRange * 2, attackRange * 2, 1f);
            attackRangeIndicator.position = transform.position;
        }
    }
}
