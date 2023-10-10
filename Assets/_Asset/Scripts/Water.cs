using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private float _timeLived = 0f;
    private float _maxTime = 3f;
    public ParticleSystem _collisionSplash;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timeLived += Time.deltaTime;

        if (_timeLived >= _maxTime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Burnable Block"))
        {
            // other.GetComponent<Block>().TryCoolDown(10f);
            GameObject newSplash = Instantiate(_collisionSplash.gameObject, transform.position, Quaternion.identity);
            ParticleSystem ps = newSplash.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                newSplash.AddComponent<CoroutineRunner>().RunCoroutine(co_PlayAndDestroy(ps, newSplash));
            }
            Destroy(gameObject);
        }    
    }

    private IEnumerator co_PlayAndDestroy(ParticleSystem ps, GameObject obj)
    {
        ps.Play();
        yield return new WaitForSeconds(1f);
        Destroy(obj);
    }
}
