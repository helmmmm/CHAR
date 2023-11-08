using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryWood : Block
{
    void Start() 
    {
        base.Start();
        _currentTemperature = 20f;
        _ignitionTemperature = Random.Range(160f, 220f);
        _heatTransferRate = 20f;
        _burnHP = Random.Range(240f, 280f);
        _burnDMG = 20f;
    }
}
