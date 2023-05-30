using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int moneyGain = 100;
    public int scoreGain = 10;
    public float speed = 10f;
    public float startHealth = 100;
    public int attackDamage = 10;
    public float defense = 0f;
    public float attackRange = 0.4f;

    private float meetSpeed = 10f;
    private Transform target;
    public float health;
    private int waypointCount = 0;

    private GameObject currentTarget;
    private bool isAttacking = false;
    private Rigidbody rb;
    private Animator animator;

    public event Action<Enemy> OnDestroyed;

    void Start()
    {
        target = Waypoints.points[waypointCount];
        health = startHealth;
        currentTarget = target.gameObject;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentTarget.CompareTag("Melee"))
        {
            MeleeUnit meleeUnit = currentTarget.GetComponent<MeleeUnit>();
            if (meleeUnit != null && !isAttacking)
            {
                if (Vector3.Distance(transform.position, currentTarget.transform.position) <= attackRange)
                {
                    meleeUnit.TakeDamage(attackDamage);
                    animator.SetTrigger("Attack");
                    StartCoroutine(AttackDelay(meleeUnit));
                }
                else
                {
                    rb.velocity = transform.forward * speed;
                }
            }
            else
            {
                GetNextWaypoint();
            }
        }
        else
        {
            Vector3 dir = currentTarget.transform.position - transform.position;
            bool isWalking = dir != Vector3.zero && Vector3.Distance(transform.position, currentTarget.transform.position) > 0.3f;
            animator.SetBool("IsWalking", isWalking);

            if (dir != Vector3.zero)
            {
                Quaternion enemyLook = Quaternion.LookRotation(dir);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, enemyLook, Time.deltaTime * meetSpeed);
                transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

                rb.velocity = transform.forward * speed;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }

            if (Vector3.Distance(transform.position, currentTarget.transform.position) <= 0.4f)
            {
                GetNextWaypoint();
            }
        }
    }

    void GetNextWaypoint()
    {
        waypointCount++;

        if (waypointCount >= Waypoints.points.Length)
        {
            Die();
            return;
        }

        target = Waypoints.points[waypointCount];
        currentTarget = target.gameObject;
        animator.SetBool("IsWalking", true);

        Debug.Log("Next waypoint target: " + target.position);
        Debug.Log("Current target: " + currentTarget);
    }

    public void TakeDamage(float amount)
    {
        health -= amount * 100 / (100 + defense);

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (currentTarget.CompareTag("Melee"))
        {
            MeleeUnit meleeUnit = currentTarget.GetComponent<MeleeUnit>();
            if (meleeUnit != null)
            {
                meleeUnit.StopAttacking();
            }
        }

        Destroy(gameObject);
    }

    public void StopAttacking()
    {
        isAttacking = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Melee"))
        {
            MeleeUnit meleeUnit = other.GetComponent<MeleeUnit>();
            if (meleeUnit != null && !isAttacking)
            {
                rb.velocity = Vector3.zero;
                animator.SetTrigger("Attack");
                StartCoroutine(AttackDelay(meleeUnit));
            }
        }
    }

    IEnumerator AttackDelay(MeleeUnit meleeUnit)
    {
        isAttacking = true;

        float attackDelay = 0.6f;
        yield return new WaitForSeconds(attackDelay);

        if (currentTarget.CompareTag("Melee") && meleeUnit != null && meleeUnit.health > 0)
        {
            meleeUnit.TakeDamage(attackDamage);
        }

        isAttacking = false;
        GetNextWaypoint();
    }
}
