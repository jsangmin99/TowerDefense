using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Money;
    public int startMoney;

    public static float Lives;
    public float startLives;

    void Start()
    {
        Money = startMoney;
        Lives = startLives;
    }
}
