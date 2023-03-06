using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    static public TrapManager i;

    List<Trap> _traps;

    private void Awake()
    {
        i = this;
    }

    public void Init()
    {
        Trap[] traps = GetComponentsInChildren<Trap>();
        _traps = new List<Trap>(traps);
    }

    void Update()
    {
        
    }

    public void AllTrapRestore()
    {
        // 모든 트랩 초기화
        foreach(Trap trap in _traps)
        {
            trap.Restore();
        }
    }
}
