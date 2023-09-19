using System.Collections;
using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.External;
using Niantic.ARDK.Utilities.Input.Legacy;
using UnityEngine;
using VoxelToolkit;

public class CubeSpawner : MonoBehaviour
{
    public GameObject CubePrefab;  // The prefab for the cube you want to spawn.
    public float spawnInterval = 2f;  // Interval in seconds to spawn cubes.
    private float _spawnTimer = 0f;
    
    private IARSession _session;  // The AR session.
    private Vector3 _circleCenter;
    private float _radius;
    private ARHitTester _hitTester;
    public ARHitTester HitTester => _hitTester ?? (_hitTester = GameObject.Find("SceneManager").GetComponent<ARHitTester>());
    
    
    void Start()
    {
        _circleCenter = gameObject.transform.position;
        _radius = gameObject.transform.localScale.x / 2;

        Debug.Log("Circle center: " + _circleCenter);
    }

    private void Update() 
    {
        if (HitTester._levelPlaced)
        {
            if (_spawnTimer >= spawnInterval)
            {
                SpawnCube();
                _spawnTimer = 0f;
            }
            else
                _spawnTimer += Time.deltaTime;
        }
    }

    void SpawnCube()
    {
        Vector3 spawnPos = GetRandomPos(_circleCenter, _radius);
        GameObject cube = Instantiate(CubePrefab, spawnPos, Quaternion.identity);
        cube.transform.parent = gameObject.transform;
    }

    private Vector3 GetRandomPos(Vector3 circleCenter, float radius)
    {
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(0f, radius);
        Vector3 spawnPos;

        spawnPos.x = circleCenter.x + distance * Mathf.Cos(angle * Mathf.Deg2Rad);
        spawnPos.y = circleCenter.y + 0.05f;
        spawnPos.z = circleCenter.z + distance * Mathf.Sin(angle * Mathf.Deg2Rad);

        Debug.Log("Spawned cube at: " + spawnPos);
        
        return spawnPos;
    }
}
