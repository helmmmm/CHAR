using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterShooter : MonoBehaviour
{
    private GameObject _waterPrefab;
    private Camera _mainCamera;

    private float _timeSinceLastShot = 0f;
    private float _fireRate = 0.025f;
    private int _streamCount = 3;
    private float _spreadAngle = 3f;

    // Start is called before the first frame update
    void Start()
    {
        _waterPrefab = Resources.Load<GameObject>("Prefabs/Water");
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _timeSinceLastShot += Time.deltaTime;

            if (_timeSinceLastShot >= _fireRate)
            {
                ShootWater();
                _timeSinceLastShot = 0f;
            }
        }
    }

    private void ShootWater()
    {
        GameObject water = Instantiate(_waterPrefab, _mainCamera.transform.position, Quaternion.identity);
        // water.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));

        // Vector3 direction = Quaternion.Euler(0, _spreadAngle * (i - _streamCount / 2), 0) * _mainCamera.transform.forward;

        Rigidbody rb = water.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0f, 0.3f, 0f);
        float force = 300.0f;
        rb.AddForce(_mainCamera.transform.forward * force);
    }
}
