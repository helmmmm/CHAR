using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryWood : Block
{
    void Start() 
    {
        base.Start();
        _currentTemperature = 20f;
        _ignitionTemperature = Random.Range(160f, 400f);
        _heatTransferRate = 40f;
        _burnHP = Random.Range(480f, 840f);
        _burnDMG = 0f;
    }
}
