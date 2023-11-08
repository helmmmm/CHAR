using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : Block
{
    void Start() 
    {
        base.Start();
        _currentTemperature = 20f;
        _ignitionTemperature = Random.Range(480f, 600f);
        _heatTransferRate = 20f;
        _burnHP = Random.Range(540f, 680f);
        _burnDMG = 20f;
    }
}
