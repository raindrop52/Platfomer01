using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    [SerializeField] float _speed;
    Vector2 _pos;

    void Awake()
    {
        _pos = transform.position;
    }

    void Update()
    {
        transform.position = new Vector2(_pos.x + Mathf.PingPong(Time.time * _speed, 20f), _pos.y);
    }
}
