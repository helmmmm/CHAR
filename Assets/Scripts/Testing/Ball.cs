using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float timeLived = 0f;
    private float maxTime = 5f;

    // Update is called once per frame
    void Update()
    {
        timeLived += Time.deltaTime;

        if (timeLived >= maxTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.tag == "FireBlock")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
