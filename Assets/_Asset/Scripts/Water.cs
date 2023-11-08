using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private float _timeLived = 0f;
    private float _maxTime = 3f;

    void Update()
    {
        _timeLived += Time.deltaTime;

        if (_timeLived >= _maxTime)
            Destroy(gameObject);

        UpdateOrientation();
    }

    private void OnTriggerEnter(Collider other) 
    {
        ScoreManager.Instance._totalWaterCount++;
        if (other.gameObject.CompareTag("Burning Block"))
        {
            ScoreManager.Instance._hitWaterCount++;
        }    
    }

    private void UpdateOrientation()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 velocity = rb.velocity;
            if (velocity != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(velocity);
            }
        }
    }
}
