using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WaterShooter : MonoBehaviour
{
    // private ObjectPool<GameObject> _waterPool;
    public GameObject _shootingPoint; // Reference to the child object
    private GameObject _waterPrefab;
    private Camera _mainCamera;

    private float _timeSinceLastShot = 0f;
    private float _fireRate = 0.02f;
    public float _firePower = 80f;

    private float _capacity = 100f;
    private float _currentWaterLevel;

    void Start()
    {
        _mainCamera = Camera.main;
        _waterPrefab = Resources.Load<GameObject>("Prefabs/Water Unit");
        _currentWaterLevel = _capacity;

        // _waterPool = new ObjectPool<GameObject>(() => 
        //     Instantiate(_waterPrefab), 
        //     water => water.SetActive(true), 
        //     water => water.SetActive(false), 
        //     null,
        //     true, 
        //     5,
        //     10
        // );

        if (_shootingPoint == null)
        {
            Debug.LogError("Shooting point is not set!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _timeSinceLastShot += Time.deltaTime;

            if (_timeSinceLastShot >= _fireRate && _currentWaterLevel > 0f)
            {
                ShootWater();
                _currentWaterLevel -= 1f;
                _timeSinceLastShot = 0f;
            }
        }
        else
        {
            if (_currentWaterLevel < _capacity)
            {
                _currentWaterLevel += 10f * Time.deltaTime;
            }
        }
    }

    private void ShootWater()
    {
        GameObject water = Instantiate(_waterPrefab);
        water.transform.position = _shootingPoint.transform.position;

        Quaternion shootAngleY = Quaternion.AngleAxis(-10, transform.up);
        Quaternion shootAngleX = Quaternion.AngleAxis(-15, transform.right);
        Vector3 shootDirection = shootAngleX * shootAngleY * transform.forward;

        Rigidbody rb = water.GetComponent<Rigidbody>();
        rb.AddForce(shootDirection * _firePower);
    }

}
