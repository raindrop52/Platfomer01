using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : Obstacle
{
    [SerializeField] float _upDist = 2f;
    [SerializeField] float _speed = 1f;

    Vector2 _goal;

    public override void Init(float time = 1F)
    {
        base.Init(time);

        _goal = new Vector2(transform.position.x, transform.position.y + _upDist);
    }

    public void Grow()
    {
        StartCoroutine(IGrowUp());
    }

    IEnumerator IGrowUp()
    {
        while(true)
        {
            transform.position = Vector2.MoveTowards(transform.position, _goal, Time.deltaTime * _speed);

            if(Vector2.Distance(transform.position, _goal) <= 0.01f)
            {
                break;
            }

            yield return null;
        }
    }
}
