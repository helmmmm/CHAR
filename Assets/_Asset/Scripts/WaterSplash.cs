using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplash : MonoBehaviour
{
    private float _timeLived = 0f;
    private float _maxTime = 3f;
    private GameObject _collisionSplash;

    private void Start()
    {
        _collisionSplash = Resources.Load<GameObject>("Prefabs/Water Splash");
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Burnable Block"))
        {
            // other.GetComponent<Block>().TryCoolDown(10f);
            GameObject newSplash = Instantiate(_collisionSplash, transform.position, Quaternion.identity);
            ParticleSystem ps = newSplash.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                newSplash.AddComponent<CoroutineRunner>().RunCoroutine(co_PlayAndDestroy(ps, newSplash));
            }
            // _waterShooter.ReleaseWaterToPool(gameObject);
            // Lean.Pool.LeanPool.Despawn(gameObject);
            // MyPooler.ObjectPooler.Instance.ReturnToPool("Water", gameObject);
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
