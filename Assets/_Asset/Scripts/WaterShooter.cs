using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private float _ColliderFireRate = 0.06f;
    private float _waterVFXFireRate = 0.03f;
    public float _firePower = 100f;

    private GameObject _waterGauge;
    private float _capacity = 40f;
    private float _currentWaterLevel;

    void Start()
    {
        _mainCamera = Camera.main;
        _waterPrefab = Resources.Load<GameObject>("Prefabs/Water Collider");
        _waterVFX = Resources.Load<GameObject>("Prefabs/Water VFX");
        // _waterPrefab.gameObject.transform.localScale *= LevelConfig.Instance.levelScale;
        _waterVFX.gameObject.transform.localScale *= LevelConfig.Instance.levelScale;
        _capacity = LevelConfig.Instance._waterCapacity;
        _currentWaterLevel = _capacity;
        _waterGauge = GameSceneUIManager.Instance._waterGauge;
        _waterGauge.GetComponent<Slider>().maxValue = _capacity;

        // Need Pooling!!

        if (_shootingPoint == null)
        {
            Debug.LogError("Shooting point is not set!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && GameManager.Instance.IsFireFighting())
        // if (Input.GetKey(KeyCode.Space))
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
                _currentWaterLevel += LevelConfig.Instance._waterReloadRate * Time.deltaTime;
            }
        }
        _waterGauge.GetComponent<Slider>().value = _currentWaterLevel;
        // Debug.Log($"Current water level: {_currentWaterLevel}\n");
    }

    private void Shoot(GameObject prefab)
    {
        GameObject projectile = Instantiate(prefab, _shootingPoint.transform.position, _shootingPoint.transform.rotation);
        projectile.transform.localScale *= LevelConfig.Instance.levelScale;

        Quaternion shootAngleY = Quaternion.AngleAxis(-5, transform.up);
        Quaternion shootAngleX = Quaternion.AngleAxis(-28, transform.right);
        Vector3 shootDirection = shootAngleX * shootAngleY * transform.forward;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(shootDirection * _firePower);


        ScoreManager.Instance._totalWaterCount++;
    }
}
