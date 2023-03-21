using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] float _speed = 1f;

    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, Time.deltaTime * _speed));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 다음 스테이지로 이동
            GameManager.i.GoNextScene();
        }
    }
}
