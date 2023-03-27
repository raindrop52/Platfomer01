using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : Obstacle
{
    Animator _anim;
    BoxCollider2D _col;

    Vector2 _pos;
    // 동작 체크 용
    [SerializeField] bool test = false;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _col = GetComponent<BoxCollider2D>();
        _col.enabled = false;
    }

    private void Update()
    {
        if (test)
        {
            Show(true);
            test = false;
            Grow();
        }
    }

    public override void Init()
    {
        base.Init();
        
        _pos = transform.localPosition;
    }

    public void SetPosX(float x)
    {
        transform.localPosition = new Vector2(x, _pos.y);
    }

    public void Anim_Trigger_On(int on)
    {
        if(_col != null)
        {
            if (on == 0)
            {
                _col.enabled = false;
                Show(false);
            }
            else if (on == 1)
                _col.enabled = true;
        }
    }

    public void Grow()
    {
        StartCoroutine(IGrowUp());
    }

    IEnumerator IGrowUp()
    {
        yield return new WaitForSeconds(1.5f);

        if (_anim != null)
            _anim.SetTrigger("Grow");
    }
}
