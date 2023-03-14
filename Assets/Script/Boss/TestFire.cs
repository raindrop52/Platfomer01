using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFire : MonoBehaviour
{
    [SerializeField] GameObject gObj;
    [SerializeField] int cnt;

    void Awake()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }

    void Fire()
    {
        for (int i = 0; i < cnt; i++)
        {
            GameObject go = Instantiate(gObj);
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;

            Rigidbody2D rigid = go.GetComponent<Rigidbody2D>();
            if (rigid != null)
            {
                float x = Random.Range(1f, 10f);
                float y = Random.Range(5f, 10f);

                Vector2 dir = new Vector2(-1f * x, 1f * y);
                rigid.AddForce(dir, ForceMode2D.Impulse);
            }
        }        
    }
}
