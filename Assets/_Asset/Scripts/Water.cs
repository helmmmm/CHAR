using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private float _timeLived = 0f;
    private float _maxTime = 3f;

    void Update()
    {
        _timeLived += Time.deltaTime;

        if (_timeLived >= _maxTime)
            Destroy(gameObject);
    }
}
