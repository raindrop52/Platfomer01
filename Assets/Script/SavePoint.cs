using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    SpriteRenderer _render;
    Collider2D _col;

    void Awake()
    {
        _render = GetComponent<SpriteRenderer>();
        _col = GetComponent<Collider2D>();
    }

    public void Touching()
    {
        // 세이브 포인트 트리거 off
        _col.enabled = false;
        // 세이브 포인트 위치 게임매니저에 전달
        GameManager.i.TouchSavePoint(transform.position);
        // 이벤트 동작 (스프라이트 회전)
        StartCoroutine(RotateSavePoint());
    }

    IEnumerator RotateSavePoint()
    {
        int cnt = 0;
        int maxRotate = 36;

        while (cnt <= maxRotate)
        {
            // 세이브 포인트를 돌린다.
            transform.localEulerAngles = new Vector3(0, cnt * 30, 0);

            cnt++;

            yield return new WaitForSeconds(0.1f);
        }

        // 세이브 포인트 색상 변경
        if (_render != null)
        {
            _render.color = Color.cyan;
        }
        yield return null;
    }
}
