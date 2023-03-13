using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    static public TrapManager i;

    public Transform _transObstacle;

    List<Trap> _traps;

    private void Awake()
    {
        i = this;
    }

    public void Init()
    {
        _traps = new List<Trap>();
        Trap[] traps = GetComponentsInChildren<Trap>();
        foreach(Trap trap in traps)
        {
            trap.Init();
            _traps.Add(trap);
        }
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
