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

    private float _currentTemperature = 20f; // Temperature of the voxel
    private float _ignitionTemperature = 140f; // Temperature at which it ignites
    private float _heatTransferRate = 20f; // How much heat it emits
    private float _burnHP = 220f; // Block's HP against fire
    private float _burnDMG = 20f; // Damage per second from fire

    public bool manualIgnition = false;

    private Collider _collider;
    private List<GameObject> _nearbyBlockList = new List<GameObject>();

    private SM_Block _smBlock;
    public event Action<GameObject> OnBlockBurnt; // "event" when called within class holding the event. argument type already defined by Action<>

    YieldInstruction waitOneSec = new WaitForSeconds(1.0f);
    

    // Start is called before the first frame update
    void Start()
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
        }    
    }

    void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "Burnable Block")
        {
            _nearbyBlockList.Remove(other.gameObject);
            other.gameObject.GetComponent<Block>().OnBlockBurnt -= RemoveFromNearbyBlockList;
        }    
    }

    private void RemoveFromNearbyBlockList(GameObject otherBlock)
    {
        _nearbyBlockList.Remove(otherBlock);
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
                thatBlock.GainHeat();
            }
        }
    }

    void GainHeat()
    {
        if (!_smBlock.IsRestingState)
        {
            return;
        }

        _currentTemperature += _heatTransferRate;
            
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
        GameObject fireInstance = Instantiate(_fireVFX, transform.position, Quaternion.identity);
        fireInstance.transform.parent = this.transform;
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
