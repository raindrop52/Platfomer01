using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tween : MonoBehaviour
{
    [SerializeField] protected float _fTime;
    [SerializeField] protected Transform _target;

    #region private
    private void Awake()
    {
        Show(false);
    }
    #endregion

    #region protected
    protected virtual void Start()
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
    }

    protected virtual void Show(bool show)
    {
        gameObject.SetActive(show);
    }
    #endregion

    #region public
    
    #endregion
}