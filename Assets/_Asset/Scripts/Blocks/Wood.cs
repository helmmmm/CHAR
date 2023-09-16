using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : BaseBlock
{

    private void Start()
    {
        base.ignitionThreshold = this.ignitionThreshold;
        base.burnHP = this.burnHP;
    }
}
