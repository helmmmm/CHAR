using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfig : MonoBehaviour
{
    public static LevelConfig Instance;
    public float levelScale;

    void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        levelScale = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
