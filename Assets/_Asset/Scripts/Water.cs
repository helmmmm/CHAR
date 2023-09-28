using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private float _timeLived = 0f;
    private float _maxTime = 3f;
    public int _splashParticles = 5;
    public float _splashForce = 3f;
    private GameObject _splashPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timeLived += Time.deltaTime;

        if (_timeLived >= _maxTime)
            Destroy(gameObject);
    }

    // private void OnTriggerEnter(Collider other) 
    // {
    //     for (int i = 0; i < _splashParticles; i++)
    //     {
    //         Debug.Log("Splash!");
    //         _splashPrefab = Resources.Load<GameObject>("Prefabs/Water Splash");
    //         GameObject splash = Instantiate(_splashPrefab, transform.position, Quaternion.identity);
    //         Rigidbody rb = splash.GetComponent<Rigidbody>();

    //         Vector3 randomDir = new Vector3(
    //             Random.Range(-1f, 1f), 
    //             Random.Range(-1f, 1f), 
    //             Random.Range(-1f, 1f)
    //         ).normalized;

    //         rb.AddForce(randomDir * _splashForce, ForceMode.Impulse);
    //     }
    // }
}
