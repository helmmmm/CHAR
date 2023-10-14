using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetScalar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale *= LevelConfig.Instance.levelScale;
        // if (gameObject.tag == "Water Unit")
        // {
        //     transform.localScale *= GameManager.Instance.levelScale;
        // }
    }
}
