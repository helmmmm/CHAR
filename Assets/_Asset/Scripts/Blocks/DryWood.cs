using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryWood : Block
{
    void Start() 
    {
        base.Start();
        _currentTemperature = 20f;
        _ignitionTemperature = 160f;
        _heatTransferRate = 20f;
        _burnHP = 240f;
        _burnDMG = 20f;
    }
}
