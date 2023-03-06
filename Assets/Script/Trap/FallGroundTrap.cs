using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallGroundTrap : Trap
{
    [SerializeField] float _hideY = -10.5f;
    Vector2 _pos;

    private void Awake()
    {
        _pos = transform.position;
    }

    public override void Restore()
    {
        base.Restore();

        transform.position = _pos;

        Disapear(true);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(Fall());
    }

    IEnumerator Fall()
    {
        float time = 0f;
        float max = 0.15f;
        float posX = transform.position.x - max / 2;

        while (time < 1f)
        {
            time += Time.deltaTime;
            float alpha = Mathf.PingPong(time, max);
            transform.position = new Vector2(posX + alpha, transform.position.y);

            yield return null;
        }

        while (true)
        {
            transform.Translate(Vector2.down * 0.5f);

            if(transform.position.y < _hideY)
            {
                Disapear(false);
            }

            yield return new WaitForSeconds(0.01f);
        }
    }
}
