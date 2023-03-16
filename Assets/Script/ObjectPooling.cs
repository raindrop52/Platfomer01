using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [SerializeField] GameObject _objPrefab;
    Queue<GameObject> _objectPool = new Queue<GameObject>();


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
