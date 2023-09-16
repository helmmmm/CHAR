using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : BaseBlock
{

    private void Start()
    {
        base.ignitionThreshold = this.ignitionThreshold;
        base.burnHP = this.burnHP;
    }
}
