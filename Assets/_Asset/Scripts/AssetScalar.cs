using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetScalar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.CompareTag("MainCamera"))
        {
            WaterShooter waterShooter = gameObject.GetComponent<WaterShooter>();
            waterShooter._firePower *= LevelConfig.Instance.levelScale;
        }
        else
        {
            Vector3 newScale = transform.localScale;
            newScale *= LevelConfig.Instance.levelScale;
            transform.localScale = newScale;   
        }

        // if (gameObject.CompareTag("Water") || gameObject.CompareTag("Water Unit"))
        // {
            
        // }
    }
}
