using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetScalar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 newScale = transform.localScale;
        newScale *= LevelConfig.Instance.levelScale;
        transform.localScale = newScale;
    }
}
