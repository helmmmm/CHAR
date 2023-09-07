using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleVoxel : MonoBehaviour
{
    public float ignitionThreshold; // Temperature at which it ignites
    public float burnHP; // or burnDuration?

    public void GainHeat(float heat)
    {
        ignitionThreshold -= heat;
        if (ignitionThreshold <= 0)
        {
            Burn();
        }
    }

    public void Burn()
    {
        if (burnHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
