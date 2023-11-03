using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetScalar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 newScale = transform.localScale;
        if (gameObject.CompareTag("Level Cursor"))
        {
            if (LevelConfig.Instance.levelType != "Building")
                newScale *= LevelConfig.Instance.levelScale * 5;
            else
                newScale *= LevelConfig.Instance.levelScale * 2;
        }
        
        else newScale *= LevelConfig.Instance.levelScale;
        
        transform.localScale = newScale;
    }
}
