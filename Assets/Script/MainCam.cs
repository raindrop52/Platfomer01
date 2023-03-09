using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{
    [SerializeField] float _minXCameraBoundary;
    [SerializeField] float _maxXCameraBoundary;

    void FixedUpdate()
    {
        if(GameManager.i.pPlayer != null)
        {
            if (!GameManager.i.BossOn)
            {
                Transform target = GameManager.i.pPlayer.transform;
                Vector3 targetPos = new Vector3(target.position.x, transform.position.y, transform.position.z);

                targetPos.x = Mathf.Clamp(targetPos.x, _minXCameraBoundary, _maxXCameraBoundary);

                transform.position = Vector3.Lerp(transform.position, targetPos, 0.2f);
            }
        }
    }
}
