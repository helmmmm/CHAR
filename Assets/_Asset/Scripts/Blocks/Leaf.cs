using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : Block
{
    void Start() 
    {
        base.Start();
        _currentTemperature = 20f;
        _ignitionTemperature = Random.Range(120f, 180f);
        _heatTransferRate = 20f;
        _burnHP = Random.Range(160f, 200f);
        _burnDMG = 20f;
    }
}
