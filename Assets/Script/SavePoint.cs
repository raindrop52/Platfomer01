using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    SpriteRenderer render;
    Collider2D col;

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    public void Touching()
    {
        // ���̺� ����Ʈ Ʈ���� off
        col.enabled = false;
        // ���̺� ����Ʈ ��ġ ���ӸŴ����� ����
        GameManager.i.TouchSavePoint(transform.position);
        // �̺�Ʈ ���� (��������Ʈ ȸ��)
        StartCoroutine(RotateSavePoint());
    }

    IEnumerator RotateSavePoint()
    {
        int cnt = 0;
        int maxRotate = 36;

        while (cnt <= maxRotate)
        {
            // ���̺� ����Ʈ�� ������.
            transform.localEulerAngles = new Vector3(0, cnt * 30, 0);

            cnt++;

            yield return new WaitForSeconds(0.1f);
        }

        // ���̺� ����Ʈ ���� ����
        if (render != null)
        {
            render.color = Color.cyan;
        }
        yield return null;
    }
}
