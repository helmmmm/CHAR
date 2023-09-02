using System.Collections;
using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.External;
using Niantic.ARDK.Utilities.Input.Legacy;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public Camera Camera;  // The camera used to render the scene.
    public GameObject CubePrefab;  // The prefab for the cube you want to spawn.
    public float spawnInterval = 2f;  // Interval in seconds to spawn cubes.
    
    private IARSession _session;  // The AR session.
    
    void Start()
    {
        StartCoroutine(SpawnCubeRoutine());
    }

    private void OnAnyARSessionDidInitialize(AnyARSessionInitializedArgs args)
    {
        _session = args.Session;
        _session.Deinitialized += OnSessionDeinitialized;
    }

    private void OnSessionDeinitialized(ARSessionDeinitializedArgs args)
    {
        StopAllCoroutines();
    }

    IEnumerator SpawnCubeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnRandomCube();
        }
    }

    private void SpawnRandomCube()
    {
        if (_session == null) return;

        var frame = _session.CurrentFrame;
        if (frame == null) return;

        // Generate random screen point
        Vector2 randomPoint = new Vector2(Random.Range(0, Camera.pixelWidth), Random.Range(0, Camera.pixelHeight));
        
        var results = frame.HitTest
        (
            Camera.pixelWidth,
            Camera.pixelHeight,
            randomPoint,
            ARHitTestResultType.ExistingPlane | ARHitTestResultType.EstimatedHorizontalPlane
        );
        
        if (results.Count <= 0) return;

        // Use the closest hit point to spawn a cube
        var hitPosition = results[0].WorldTransform.ToPosition();
        Instantiate(CubePrefab, hitPosition, Quaternion.identity);
    }
}
