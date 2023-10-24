using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WaterShooter : MonoBehaviour
{
    // private ObjectPool<GameObject> _waterPool;
    public GameObject _shootingPoint; // Reference to the child object
    private GameObject _waterPrefab;
    private GameObject _waterVFX;
    private Camera _mainCamera;

    private float _timeSinceLastShotCollider = 0f;
    private float _timeSinceLastShotVFX = 0f;
    private float _ColliderFireRate = 0.04f;
    private float _waterVFXFireRate = 0.02f;
    public float _firePower = 100f;

    private float _capacity = 50f;
    private float _currentWaterLevel;

    void Start()
    {
        _mainCamera = Camera.main;
        _waterPrefab = Resources.Load<GameObject>("Prefabs/Water Collider");
        _waterVFX = Resources.Load<GameObject>("Prefabs/Water VFX");
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
            _timeSinceLastShotVFX += Time.deltaTime;
            _timeSinceLastShotCollider += Time.deltaTime;

            if (_currentWaterLevel > 0f)
            {
                if (_timeSinceLastShotCollider >= _ColliderFireRate)
                {
                    // ShootWaterCollider();
                    Shoot(_waterPrefab);
                    _timeSinceLastShotCollider = 0f;
                }

                if (_timeSinceLastShotVFX >= _waterVFXFireRate)
                {
                    // ShootWaterVFX();
                    Shoot(_waterVFX);
                    _timeSinceLastShotVFX = 0f;
                }

                _currentWaterLevel -= 10f * Time.deltaTime;
            }
        }
        else
        {
            if (_currentWaterLevel < _capacity)
            {
                _currentWaterLevel += 5f * Time.deltaTime;
            }
        }
        Debug.Log($"Current water level: {_currentWaterLevel}\n");
    }

    private void Shoot(GameObject prefab)
    {
        GameObject projectile = Instantiate(prefab);
        projectile.transform.position = _shootingPoint.transform.position;

        Quaternion shootAngleY = Quaternion.AngleAxis(-10, transform.up);
        Quaternion shootAngleX = Quaternion.AngleAxis(-15, transform.right);
        Vector3 shootDirection = shootAngleX * shootAngleY * transform.forward;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(shootDirection * _firePower);
    }

    // private void ShootWaterCollider() // make into one method
    // {
    //     GameObject water = Instantiate(_waterPrefab);
    //     water.transform.position = _shootingPoint.transform.position;

    //     Quaternion shootAngleY = Quaternion.AngleAxis(-10, transform.up);
    //     Quaternion shootAngleX = Quaternion.AngleAxis(-15, transform.right);
    //     Vector3 shootDirection = shootAngleX * shootAngleY * transform.forward;

    //     Rigidbody rb = water.GetComponent<Rigidbody>();
    //     rb.AddForce(shootDirection * _firePower);
    // }

    // private void ShootWaterVFX()
    // {
    //     GameObject waterVFX = Instantiate(_waterVFX);
    //     waterVFX.transform.position = _shootingPoint.transform.position;

    //     Quaternion shootAngleY = Quaternion.AngleAxis(-10, transform.up);
    //     Quaternion shootAngleX = Quaternion.AngleAxis(-15, transform.right);
    //     Vector3 shootDirection = shootAngleX * shootAngleY * transform.forward;

    //     Rigidbody rb = waterVFX.GetComponent<Rigidbody>();
    //     rb.AddForce(shootDirection * _firePower);
    // }

}
