using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Block
{
    void Start() 
    {
        base.Start();
        _currentTemperature = 20f;
        _ignitionTemperature = 120f;
        _heatTransferRate = 20f;
        _burnHP = 240f;
        _burnDMG = 20f;
    }
}
