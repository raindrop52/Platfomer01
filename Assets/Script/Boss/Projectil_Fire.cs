using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectil_Fire : MonoBehaviour
{
    [SerializeField] GameObject _goPrefab;
    [SerializeField] int _nCount;
    [SerializeField] Vector2 _vPowerOffset;             // 주는 힘 배율
    List<Bomb> _bombs;

    void Awake()
    {
        _bombs = new List<Bomb>();

        if (_goPrefab != null)
        {
            for (int i = 0; i < _nCount; i++)
            {
                GameObject go = Instantiate(_goPrefab);
                go.transform.SetParent(transform);
                go.transform.localPosition = Vector3.zero;
                // 프리팹 비활성화
                go.SetActive(false);
                // 프리팹 목록들 리스트에 추가
                Bomb bomb = go.GetComponent<Bomb>();
                if (bomb != null)
                {
                    Vector2 min = new Vector2(i * _vPowerOffset.x, _vPowerOffset.y);
                    Vector2 max = new Vector2((i + 1) * _vPowerOffset.x, _vPowerOffset.y * 4f);
                    bomb.Init(min, max);
                    _bombs.Add(bomb);
                }
            }
        }
    }

    public void AllFire()
    {
        foreach (Bomb bomb in _bombs)
        {
            bomb.Reload();
            bomb.Fire();
        }
    }
}
