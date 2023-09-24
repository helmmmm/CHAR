using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlock : MonoBehaviour
{
    public float currentTemperature; // Temperature of the voxel
    public float heatTransferRate = 10f;    

    public float ignitionThreshold; // Temperature at which it ignites
    public float burnHP; // or burnDuration?
    public float burnDamage; // Damage per second from fire
    
    public bool atIgnitionThreshold = false;
    public bool isBurning = false; // Is the voxel on fire?
    public bool isBurnt = false; // Is the voxel burnt?
    
    private Collider collider;

    List<GameObject> nearbyBlockList = new List<GameObject>();
    

    void Start() 
    {
        collider = GetComponent<Collider>();
    }   

    void Update() 
    {
        if(isBurning)
        {
            EmitHeat();
        }

        foreach (GameObject block in nearbyBlockList)
        {
            BaseBlock thatBlock = block.GetComponent<BaseBlock>();
            // thatBlock.GainHeat();

            if (thatBlock.currentTemperature >= thatBlock.ignitionThreshold)
            {
                thatBlock.atIgnitionThreshold = true;
                thatBlock.Ignite();
            }
        }

        if (atIgnitionThreshold && !isBurning)
        {
            Ignite();
        }
        else if (atIgnitionThreshold && isBurning)
        {
            Burn();
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Burnable Block")
        {
            nearbyBlockList.Add(other.gameObject);
        }    
    }

    void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "Burnable Block")
        {
            nearbyBlockList.Remove(other.gameObject);
        }    
    }


    public void GainHeat()
    {
        if (!isBurning)
        {
            currentTemperature += heatTransferRate * Time.deltaTime;
        }
    }

    public void EmitHeat()
    {
        foreach (GameObject block in nearbyBlockList)
        {
            BaseBlock thatBlock = block.GetComponent<BaseBlock>();
            thatBlock.GainHeat();
        }
    }

    public void Ignite()
    {
        Debug.Log(gameObject.name + " Caught on fire!");
        Burn();
    }

    public void Burn()
    {
        isBurning = true;
        if (burnHP >= 0)
        {
            burnHP -=  burnDamage * Time.deltaTime;
            Debug.Log(gameObject.name + " Burning!");
        }
        else
        {
            Burnt();
        }
    }

    public void Burnt()
    {
        // isBurnt = true;
        // atIgnitionThreshold = false;
        // isBurning = false;
        Debug.Log("Burnt!");
        collider.enabled = false;
        Destroy(gameObject);
    }
}