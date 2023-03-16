using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager i;
    List<Boss> _bosses;

    private void Awake()
    {
        i = this;
    }

    public void Init()
    {
        // 보스 정보가 담긴 리스트
        Boss[] bosses = GetComponentsInChildren<Boss>();
        if (bosses.Length > 0)
            _bosses = new List<Boss>(bosses);
    }

    public void AllRestore()
    {
        foreach (Boss boss in _bosses)
            boss.Restore();
    }
}
