using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Block : MonoBehaviour
{
    Renderer rend;
    private Material _defaultMaterial;
    private Material _burningMaterial;
    private GameObject _fireVFX;
    private GameObject _fireVFXInstance = null;

    protected float _currentTemperature = 20f; // Temperature of the voxel
    protected float _ignitionTemperature = 140f; // Temperature at which it ignites
    protected float _heatTransferRate = 20f; // How much heat it emits
    protected float _burnHP = 220f; // Block's HP against fire
    protected float _burnDMG = 20f; // Damage per second from fire

    public bool manualIgnition = false;

    private Collider _collider;
    private List<GameObject> _nearbyBlockList = new List<GameObject>();

    private SM_Block _smBlock;
    public event Action<GameObject> OnBlockBurnt; // "event" when called within class holding the event. argument type already defined by Action<>

    YieldInstruction waitOneSec = new WaitForSeconds(1.0f);
    

    // Start is called before the first frame update
    protected void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;

        _defaultMaterial = GetComponent<Renderer>().materials[0];
        _burningMaterial = Resources.Load<Material>("Materials/Burning");
        
        _fireVFX = Resources.Load<GameObject>("VFX/Fire VFX");

        _smBlock = new SM_Block();
        _smBlock.Initialize();
        _smBlock.CheckComponent();

        _smBlock.BSM_State_Burnt.OnEnter += Burnt;

        _smBlock.BSM_State_Burning.OnEnter += ChangeToBurningMaterial;
        _smBlock.BSM_State_Burning.OnEnter += SpawnFireVFX;

        _collider = GetComponent<Collider>();

        StartCoroutine(co_EmitHeat());
        StartCoroutine(co_Burn());
    }

    // Update is called once per frame
    void Update()
    {
        if (manualIgnition)
        {
            _smBlock.TryChangeState(_smBlock.BSM_State_Burning);
            manualIgnition = false;
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Burnable Block")
        {
            _nearbyBlockList.Add(other.gameObject);
            other.gameObject.GetComponent<Block>().OnBlockBurnt += RemoveFromNearbyBlockList;
            other.gameObject.GetComponent<Block>().OnBlockBurnt += TrySpawnFireVFX;
        }    
    }

    void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "Burnable Block")
        {
            _nearbyBlockList.Remove(other.gameObject);
            other.gameObject.GetComponent<Block>().OnBlockBurnt -= RemoveFromNearbyBlockList;
            other.gameObject.GetComponent<Block>().OnBlockBurnt -= TrySpawnFireVFX;
        }
    }

    private void RemoveFromNearbyBlockList(GameObject otherBlock)
    {
        _nearbyBlockList.Remove(otherBlock);
    }

    private void TrySpawnFireVFX(GameObject otherBlock)
    {
        if(_fireVFXInstance = null)
            SpawnFireVFX();
    }

    IEnumerator co_EmitHeat()
    {
        while (true)
        {
            yield return waitOneSec;
            if (!_smBlock.IsBurningState)
            {
                continue;
            }
            foreach (GameObject block in _nearbyBlockList)
            {
                Block thatBlock = block.GetComponent<Block>();
                thatBlock.GainHeat(transform.position.y);
            }
        }
    }

    void GainHeat(float heatSourcePosY)
    {
        float heatFactor = 1f;

        if (!_smBlock.IsRestingState)
        {
            return;
        }

        if (UnityEngine.Random.Range(0, 100) > 40)
        {
            return;
        }

        if (Mathf.Abs(heatSourcePosY - transform.position.y) < 0.01f)
        {
            heatFactor = 1f;
        }
        else if (heatSourcePosY > transform.position.y)
        {
            heatFactor = 0.5f;
        }
        else if (heatSourcePosY < transform.position.y)
        {
            heatFactor = 1.5f;
        }

        _currentTemperature += _heatTransferRate * heatFactor;
            
        if (_currentTemperature >= _ignitionTemperature)
        {
            _smBlock.TryChangeState(_smBlock.BSM_State_Burning);
        }
    }

    IEnumerator co_Burn()
    {
        while (true)
        {
            yield return waitOneSec;
            if (!_smBlock.IsBurningState)
            {
                continue;
            }

            _burnHP -= _burnDMG;
            Debug.Log($"[{gameObject.name}] HP: {_burnHP}\n");

            if (_burnHP <= 0)
            {
                _smBlock.TryChangeState(_smBlock.BSM_State_Burnt);
            }
        }
    }

    void ChangeToBurningMaterial()
    {
        rend.material = _burningMaterial;
    }

    void SpawnFireVFX()
    {
        // if (contact list has 6 || has 5 and the bottom is open)
        if (_nearbyBlockList.Count == 6)
            return;
        
        if (_nearbyBlockList.Count == 5)
        {
            bool hasBottom = false;
            foreach(GameObject block in _nearbyBlockList)
            {
                if (block.transform.position.y < transform.position.y)
                    hasBottom = true;
            }
            if (!hasBottom)
                return;
        }

        _fireVFXInstance = Instantiate(_fireVFX, transform.position, Quaternion.identity);
        _fireVFXInstance.transform.parent = this.transform;
    }

    void Burn()
    {
        if (_smBlock.IsBurningState)
        {
            _burnHP -= _burnDMG * Time.deltaTime;

            if (_burnHP <= 0)
            {
                _smBlock.TryChangeState(_smBlock.BSM_State_Burnt);
            }    
        }
    }

    void Burnt()
    {
        Debug.Log($"[{gameObject.name}] is Burnt");
        OnBlockBurnt?.Invoke(gameObject); // ? to check if OnBlockBurnt is null
        Destroy(gameObject);
    }
}