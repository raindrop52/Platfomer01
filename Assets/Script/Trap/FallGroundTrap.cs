using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallGroundTrap : Trap
{
    [SerializeField] float _hideY = -10.5f;
    [SerializeField] float _readyTime;
    [SerializeField] float _gravity;
    
    Vector2 _pos;
    Rigidbody2D _rigid;
    PolygonCollider2D _polyCol;

    private void Awake()
    {
        _pos = transform.position;
        _rigid = GetComponent<Rigidbody2D>();
        _polyCol = GetComponent<PolygonCollider2D>();
    }

    public override void Restore()
    {
        base.Restore();

        transform.position = _pos;
        _rigid.bodyType = RigidbodyType2D.Kinematic;
        _polyCol.enabled = true;

        Disapear(true);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            StartCoroutine(Fall());
    }

    IEnumerator Fall()
    {
        float time = 0f;
        float max = 0.15f;
        float posX = transform.position.x - max / 2;

        while (time < _readyTime)
        {
            time += Time.deltaTime;
            float alpha = Mathf.PingPong(time, max);
            transform.position = new Vector2(posX + alpha, transform.position.y);

            yield return null;
        }

        _rigid.bodyType = RigidbodyType2D.Dynamic;
        _rigid.gravityScale = _gravity;
        _polyCol.enabled = false;

        while (true)
        {
            if (transform.position.y < _hideY)
            {
                Disapear(false);
                break;
            }

            yield return new WaitForSeconds(0.01f);
        }
    }
}
