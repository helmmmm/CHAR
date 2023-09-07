using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : SimpleVoxel
{
    public float ignitionThreshold = 30f;
    public float burnHP = 120f;

    private void Start()
    {
        base.ignitionThreshold = this.ignitionThreshold;
        base.burnHP = this.burnHP;
    }
}
