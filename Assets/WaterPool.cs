using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPool : MonoBehaviour
{
    public static WaterPool instance;
    
    private List<GameObject> _pooledObj = new List<GameObject>();
    private int _poolSize = 30;

    [SerializeField] private GameObject _waterPrefab;
    private void Awake() 
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obj = Instantiate(_waterPrefab);
            obj.SetActive(false);
            _pooledObj.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _pooledObj.Count; i++)
        {
            if (!_pooledObj[i].activeInHierarchy)
                return _pooledObj[i];
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
