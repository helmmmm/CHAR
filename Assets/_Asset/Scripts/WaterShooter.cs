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
    public float _firePower = 300f;

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
        // Debug.Log(_currentWaterLevel);
    }

    private void ShootWater()
    {
        // Calculate the position for the bottom-right corner of the screen in screen coordinates
        Vector3 bottomRightScreen = new Vector3(Screen.width, 10, 2); // Z value represents distance from the camera, you may adjust this value
        
        // Convert screen to world coordinates
        Vector3 bottomRightWorld = _mainCamera.ScreenToWorldPoint(bottomRightScreen);

        _shootingPoint.transform.position = bottomRightWorld;

        // Instantiate water particles at that position
        GameObject water = Instantiate(_waterPrefab, _shootingPoint.transform.position, Quaternion.identity);
        // GameObject water = _waterPool.Get();
        // waterScript._waterShooter = this;
        // GameObject water = Lean.Pool.LeanPool.Spawn(_waterPrefab, _shootingPoint.transform.position, Quaternion.identity);
        // GameObject water = MyPooler.ObjectPooler.Instance.GetFromPool("Water", _shootingPoint.transform.position, Quaternion.identity);
        

        // Calculate the direction to shoot in towards the screen center
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 1.4f, 5); // Z value represents distance from the camera
        Vector3 centerWorld = _mainCamera.ScreenToWorldPoint(screenCenter + new Vector3(0f, 3f, 0f));
        Vector3 shootDirection = (centerWorld - bottomRightWorld).normalized;

        float lerpValue = 0.3f; // You can adjust this value to make the angle more or less steep
        Vector3 angledShootDirection = Vector3.Lerp(shootDirection, Vector3.up, lerpValue).normalized;


        // Apply the velocity and force
        Rigidbody rb = water.GetComponent<Rigidbody>();
        float force = _firePower;
        rb.velocity = angledShootDirection * 0.3f; // Adjust speed as necessary
        rb.AddForce(angledShootDirection * force);
    }

    // public void ReleaseWaterToPool(GameObject water)
    // {
    //     _waterPool.Release(water);
    // }
}
