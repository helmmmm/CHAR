using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : Block
{
    void Start() 
    {
        base.Start();
        _currentTemperature = 20f;
        _ignitionTemperature = 200f;
        _heatTransferRate = 20f;
        _burnHP = 30000f;
        _burnDMG = 20f;
    }
}
