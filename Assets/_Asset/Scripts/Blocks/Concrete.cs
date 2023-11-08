using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concrete : Block
{
    void Start() 
    {
        base.Start();
        _currentTemperature = 20f;
        _ignitionTemperature = Random.Range(540f, 720f);
        _heatTransferRate = 20f;
        _burnHP = Random.Range(640f, 800f);
        _burnDMG = 20f;
    }
}
