using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tween_Item : Tween
{
    [SerializeField] Ease _easeX;
    [SerializeField] Ease _easeY;
    [SerializeField] float _fOffsetY;
    public Item _item;

    void ShowItem()
    {
        Show(false);
        if (_item != null)
            _item.Equip(GameManager.i.pPlayer);
    }

    void SetPlayer()
    {
        transform.position = Camera.main.WorldToScreenPoint(GameManager.i.pPlayer.transform.position + new Vector3(0f, _fOffsetY, 0f));
    }

    public void Move()
    {
        Show(true);

        SetPlayer();

        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(transform.DOMoveX(_target.position.x, _fTime).SetEase(_easeX))
            .Join(transform.DOMoveY(_target.position.y, _fTime).SetEase(_easeY))
            .OnComplete(() => ShowItem());
    }
}
