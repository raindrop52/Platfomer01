using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{
    [SerializeField] float minXCameraBoundary;
    [SerializeField] float maxXCameraBoundary;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if(GameManager.i.player != null)
        {
            if (!GameManager.i.BossOn)
            {
                Transform target = GameManager.i.player.transform;
                Vector3 targetPos = new Vector3(target.position.x, transform.position.y, transform.position.z);

                targetPos.x = Mathf.Clamp(targetPos.x, minXCameraBoundary, maxXCameraBoundary);

                transform.position = Vector3.Lerp(transform.position, targetPos, 0.2f);
            }
        }
    }
}
