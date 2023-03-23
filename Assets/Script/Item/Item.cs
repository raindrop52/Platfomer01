using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected Player _player;

    public virtual void Equip(Player player)
    {
        _player = player;

        Show(false);
        
        if(UIManager.i != null)
            UIManager.i.EquipItem(GetType().ToString());
    }

    public virtual void Use()
    {
        Show(true);
    }

    protected void Show(bool show)
    {
        gameObject.SetActive(show);
    }
}
