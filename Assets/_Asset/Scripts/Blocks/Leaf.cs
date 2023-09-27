using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : Block
{
    void Start() 
    {
        base.Start();
        _currentTemperature = 20f;
        _ignitionTemperature = 80f;
        _heatTransferRate = 20f;
        _burnHP = 100f;
        _burnDMG = 20f;
    }
}
