using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Niantic.ARDK.AR;
using Niantic.ARDK.AR.Depth;
using VoxelToolkit.MagicaVoxel;

public class CubeSpawner : MonoBehaviour
{
    // public GameObject cubePrefab;
    // private IARSession _ARSession;
    // private float timeSinceLastSpawn = 0f;
    // public float spawnInterval = 2f;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     _ARSession.DepthManager.EnableFeature(DepthFeature.Surfaces);
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     timeSinceLastSpawn += Time.deltaTime;

    //     if (timeSinceLastSpawn >= spawnInterval)
    //     {
    //         SpawnCube();
    //         timeSinceLastSpawn = 0f;
    //     }
    // }

    // private void SpawnCube()
    // {
    //     var surfaces = _ARSession.DepthManager.Surfaces;

    //     if (surfaces.Count > 0)
    //     {
    //         var surface = surfaces[0];

    //         Vector3 randomPoint = GetRandomPointOnSurface(surface);

    //         Instantiate(cubePrefab, randomPoint, Quaternion.identity);
    //     }
    // }

    // private Vector3 GetRandomPointOnSurface()
    // {
    //     Vector3[] vertices = surface.Mesh.vertices;

    //     Vector3 randomVertex = vertices[Random.Range(0, vertices.Length)];

    //     Vector3 worldPosition = surface.Transform.TransformPoint(randomVertex);

    //     return worldPosition;
    // }
}
