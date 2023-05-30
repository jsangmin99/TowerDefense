using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // 싱글턴 인스턴스
    private static PlayerStats instance;
    public static PlayerStats Instance { get { return instance; } }

    // 플레이어의 점수와 골드
    private int score = 0;
    private int money = 0;

    private void Awake()
    {
        // 인스턴스 생성 및 중복 생성 방지
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }
}
