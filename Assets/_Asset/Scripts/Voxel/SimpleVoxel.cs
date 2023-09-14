using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleVoxel : MonoBehaviour
{
    public float ignitionThreshold; // Temperature at which it ignites
    public float burnHP; // or burnDuration?
    public float currentTemperature; // Temperature of the voxel
    public bool isOnFire; // Is the voxel on fire?
    public bool isBurnt; // Is the voxel burnt?
    private Fire fire;
    private float burnDamage; // Damage per second from fire

    public void GainHeat(float heat)
    {
        currentTemperature += heat * Time.deltaTime;
        if (currentTemperature >= ignitionThreshold)
        {
            Ignite();
        }
    }

    public void Ignite()
    {
        isOnFire = true;
        Debug.Log("Caught on fire!");
        if (burnHP >= 0)
        {
            burnHP -=  burnDamage * Time.deltaTime;
        }
    }
}
