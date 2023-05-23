using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeUnit : MonoBehaviour
{
    //public GameObject tower;
    public float speed = 10f;

    public float startHealth = 100;

    private float meetSpeed = 10f;

    private Transform target;
    private float health;

    //public Image healthBar;
    public float defense = 0f;

    void Start()
    {

        health = startHealth;
    }
    
    void Update()
    {
        
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        //Enemy look Foward. Reference youtuber Brackeys.
        // And message pops
        if (dir != Vector3.zero)
        {
            Quaternion enemyLook = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, enemyLook, Time.deltaTime * meetSpeed).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
        
        

    }


    public void TakeDamage(float amount)
    {
        //health -= amount;

        // 피해량 = 데미지(amount) + 100 / (100+방어력)
        health -= amount * 100 / (100 + defense);

        //healthBar.fillAmount = health / startHealth;

        if(health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
