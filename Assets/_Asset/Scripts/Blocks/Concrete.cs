using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concrete : Block
{
    void Start() 
    {
        base.Start();
        _currentTemperature = 20f;
        _ignitionTemperature = 100f;
        _heatTransferRate = 20f;
        _burnHP = 240f;
        _burnDMG = 20f;
    }
}
