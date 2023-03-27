using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager i;

    [SerializeField] Text _txtDie;
    [SerializeField] Image _imgItemSpr;

    void Awake()
    {
        i = this;
    }

    public void Init()
    {
        Restore();
    }

    public void Restore()
    {
        ShowDieText(false);
        _imgItemSpr.sprite = null;
        _imgItemSpr.color = new Color(1f,1f,1f,0f);
    }

    public void EquipItem(string type)
    {
        _imgItemSpr.color = new Color(1f, 1f, 1f, 1f);
        string name = type.ToLower();
        Sprite sprite = Resources.Load<Sprite>(name);
        _imgItemSpr.sprite = sprite;
    }

    void Update()
    {
        if (GameManager.i != null && GameManager.i.GameOver)
        {
            if(!_txtDie.gameObject.activeSelf)
                ShowDieText(true);
        }
    }

    void ShowDieText(bool show)
    {
        _txtDie.gameObject.SetActive(show);
    }
}
