using System.Collections.Generic;
using UnityEngine;

public class WaterParticles : MonoBehaviour
{
    private ParticleSystem _ps;
    public List<Collider> _colliderList; // List to keep track of colliders
    private List<ParticleSystem.Particle> _allParticles;

    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
        _allParticles = new List<ParticleSystem.Particle>();

        if (_ps == null)
        {
            Debug.LogError("No ParticleSystem found!");
            return;
        }

        // Find all GameObjects with the tag "BurnableBlock"
        GameObject[] burnableBlocks = GameObject.FindGameObjectsWithTag("Burnable Block");

        _colliderList = new List<Collider>();  // Initialize the list

        // Fill the list with colliders
        for (int i = 0; i < burnableBlocks.Length; i++)
        {
            Collider collider = burnableBlocks[i].GetComponent<Collider>();

            if (collider != null)
            {
                _colliderList.Add(collider); // Add to the list
            }
            else
            {
                Debug.LogError("No Collider found in GameObject with tag BurnableBlock!");
            }
        }
        

        Debug.Log($"ParticleSystem initialized: {_ps != null}");
    Debug.Log($"Number of colliders found: {_colliderList.Count}");
        // At this point, colliderList contains all the colliders of GameObjects with the tag "BurnableBlock"
        // You can use this list to implement your custom logic for interaction with the Particle System
    }

    void Update()
    {
        // // Step 1: Get all the particles from the Particle System
        // ParticleSystem.Particle[] particles = new ParticleSystem.Particle[_ps.particleCount];
        // int particleCount = _ps.GetParticles(particles);

        // // Step 2: Loop through all particles and all colliders to find intersections
        // for (int p = 0; p < particleCount; p++)
        // {
        //     Vector3 particlePosition = particles[p].position;

        //     foreach (Collider col in _colliderList)
        //     {
        //         if (col.bounds.Contains(particlePosition))
        //         {
        //             // Mark this particle as inside one of the colliders
        //             _allParticles.Add(particles[p]);
        //             break; // No need to check other colliders for this particle
        //         }
        //     }
        // }

        // // Step 3: Remove particles that are inside colliders
        // foreach (ParticleSystem.Particle particle in _allParticles)
        // {
        //     for (int i = 0; i < particleCount; i++)
        //     {
        //         if (particles[i].position == particle.position)
        //         {
        //             particles[i].remainingLifetime = 0;
        //             break;
        //         }
        //     }
        // }

        // // Update the Particle System with the modified array
        // _ps.SetParticles(particles, particleCount);

        // // Clear the list of inside particles for the next frame
        // _allParticles.Clear();
    }


    void OnParticleTrigger()
    {
        // List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
        // int numEnter = ParticlePhysicsExtensions.GetTriggerParticles(_ps, ParticleSystemTriggerEventType.Enter, enter);

        // for (int i = 0; i < numEnter; i++)
        // {
        //     ParticleSystem.Particle particle = enter[i];
        //     particle.remainingLifetime = 0;  // Set remaining lifetime to 0 to destroy the particle
        //     enter[i] = particle;
        // }

        // ParticlePhysicsExtensions.SetTriggerParticles(_ps, ParticleSystemTriggerEventType.Enter, enter, 0, numEnter);
        // Debug.Log($"Number of particles entering: {numEnter}");
        Debug.Log("Touched");
    }
}
