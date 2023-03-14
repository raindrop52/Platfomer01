using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : Obstacle
{
    [SerializeField] float _fUpDist = 2f;
    [SerializeField] float _fMaxSpeed = 0.05f;
    [SerializeField] bool test = false;
    Vector2 _pos;

    private void Update()
    {
        if (test)
        {
            test = false;
            _pos = transform.localPosition;
            Grow();
        }
    }

    public override void Init(float time = 1F)
    {
        base.Init(time);
        _pos = transform.localPosition;
    }

    public void SetPosX(float x)
    {
        transform.localPosition = new Vector2(x, _pos.y);
    }

    public void Grow()
    {
        StartCoroutine(IGrowUp());
    }

    IEnumerator IGrowUp()
    {
        yield return new WaitForSeconds(0.75f);
        Vector2 pos = new Vector2(transform.localPosition.x, transform.localPosition.y + _fUpDist);
        
        while (true)
        {
            float speed = Random.Range(0.01f, _fMaxSpeed);

            if (Vector2.Distance(transform.localPosition, pos) <= 0.02f)
            {
                break;
            }

            transform.Translate(Vector2.up * speed);

            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        gameObject.SetActive(false);
    }
}
