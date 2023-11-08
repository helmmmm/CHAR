using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Block
{
    void Start() 
    {
        base.Start();
        _currentTemperature = 20f;
        _ignitionTemperature = Random.Range(180f, 240f);
        _heatTransferRate = 20f;
        _burnHP = Random.Range(260f, 300f);
        _burnDMG = 20f;
    }
}
