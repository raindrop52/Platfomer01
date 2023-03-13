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
    Vector2 _pos;

    private void Awake()
    {
        _render = GetComponentInChildren<SpriteRenderer>();
    }

    [SerializeField] bool bTest = false;
    private void Update()
    {
        if (bTest)
        {
            transform.Translate(Vector2.right * _speed * Time.deltaTime);
            //bTest = false;
            //StartCoroutine(Fire());
        }
    }

    public override void Init(float time = 1F)
    {
        base.Init(time);

        gameObject.SetActive(false);

        // ÁÂ¿ì
        if (transform.GetChild(0).eulerAngles.z != 0)
            _bHorizontal = true;

        if (_pos == Vector2.zero)
            _pos = transform.position;
    }

    public void Fire()
    {

        if (_speed > 0)
        {
            if (gameObject.activeSelf == false)
                gameObject.SetActive(true);
            StartCoroutine(IFire());
        }
    }

    IEnumerator IFire()
    {
        transform.position = _pos;

        while (true)
        {
            if(_bHorizontal)
            {
                if (Mathf.Abs(Vector2.Distance(transform.position, _pos)) >= _speed)
                {
                    break;
                }

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

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeathObj"))
        {
            gameObject.SetActive(false);
        }
    }
}
