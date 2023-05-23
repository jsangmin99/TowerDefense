using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int moneyGain = 100;
    public float speed = 10f;
    public float startHealth = 100;
    public int attackDamage = 10;

    public float defense = 0f;

    
    private float meetSpeed = 10f;
    private Transform target;
    public float health;
    private int waypointCount = 0;

    private GameObject currentTarget; // 현재 타겟

    private bool isAttacking = false; // 공격 중인지 여부를 나타내는 변수
    private Rigidbody rb;

    void Start()
    {
        target = Waypoints.points[0];
        health = startHealth;
        currentTarget = target.gameObject;

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // 물리 시뮬레이션에 영향을 받지 않도록 Rigidbody를 kinematic으로 설정
    }

    void Update()
    {
;
        if (currentTarget.CompareTag("Melee") && currentTarget.GetComponent<MeleeUnit>().health <= 0)
        {
            currentTarget = target.gameObject;
        }

        Vector3 dir = currentTarget.transform.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (dir != Vector3.zero)
        {
            Quaternion enemyLook = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, enemyLook, Time.deltaTime * meetSpeed).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
        
        if (Vector3.Distance(transform.position, currentTarget.transform.position) <= 0.3f)
        {
            if (currentTarget.CompareTag("Melee"))
            {
                MeleeUnit meleeUnit = currentTarget.GetComponent<MeleeUnit>();
                if (meleeUnit != null && !isAttacking)
                {
                    meleeUnit.TakeDamage(attackDamage);
                    StartCoroutine(AttackDelay());
                }
            }
            else
            {
                GetNextWaypoint();
            }
        }
    Debug.Log("Enemy: currentTarget = " + currentTarget);
    Debug.Log("Enemy: isAttacking = " + isAttacking);
    }

    void GetNextWaypoint()
    {
        if (waypointCount >= Waypoints.points.Length - 1)
        {
            waypointCount = 1;
            target = Waypoints.points[waypointCount];
        }
        else
        {
            waypointCount++;
            target = Waypoints.points[waypointCount];
        }
        currentTarget = target.gameObject;
    }

    public void TakeDamage(float amount)
    {
        health -= amount * 100 / (100 + defense);

        if (health <= 0f)
        {
            Die();
        }
    }

    IEnumerator AttackDelay()
    {
        isAttacking = true;

        // 공격 대기 시간 설정
        float attackDelay = 1f; // 적당한 값으로 설정해주세요

        yield return new WaitForSeconds(attackDelay);

        isAttacking = false;
                Debug.Log("Enemy: Attack finished");

    }

    void Die()
    {
        PlayerStats.Money += moneyGain;
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Melee"))
        {
            MeleeUnit meleeUnit = other.GetComponent<MeleeUnit>();
            if (meleeUnit != null && !isAttacking)
            {
                currentTarget = other.gameObject;
                StartCoroutine(AttackDelay());
            }
        }
    }
}
