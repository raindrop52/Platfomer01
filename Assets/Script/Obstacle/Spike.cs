using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Obstacle
{
    bool _bHorizontal = false;
    SpriteRenderer _render;
    [SerializeField] float _speed;
    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    public override void Init(float time = 1F)
    {
        base.Init(time);

        _render = GetComponent<SpriteRenderer>();

        // ÁÂ¿ì
        if (transform.eulerAngles.y != 0)
            _bHorizontal = true;

        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        while (true)
        {
            if(_bHorizontal)
            {
                // ÁÂÃø ÀÌµ¿
                if(_render.flipY)
                    transform.Translate(Vector2.left * _speed * Time.deltaTime);
                // ¿ìÃøÀÌµ¿
                else
                    transform.Translate(Vector2.right * _speed * Time.deltaTime);
            }
            else
            {
                // »ó½Â
                if (_render.flipY)
                    transform.Translate(Vector2.up * _speed * Time.deltaTime);
                // ÇÏ°­
                else
                    transform.Translate(Vector2.down * _speed * Time.deltaTime);
            }
            
            yield return null;
        }
    }
}
