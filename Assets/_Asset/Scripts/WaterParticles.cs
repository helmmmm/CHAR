using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParticles : MonoBehaviour
{
    private float _coolingRate = 80f; // How much heat it loses per second

    private void Start() 
    {
        
    }

    private void OnParticleCollision(GameObject other) 
    {
        if (other.CompareTag("Burnable Block"))
        {
            other.GetComponent<Block>().TryCoolDown(_coolingRate);
        }
    }
}
