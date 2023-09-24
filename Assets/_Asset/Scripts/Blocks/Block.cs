using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Block : MonoBehaviour
{
    private float _currentTemperature = 30f; // Temperature of the voxel
    private float _ignitionTemperature = 90f; // Temperature at which it ignites
    private float _heatTransferRate = 30f; // How much heat it emits
    private float _burnHP = 120f; // Block's HP against fire
    private float _burnDMG = 30f; // Damage per second from fire

    public bool manualIgnition = false;

    private Collider _collider;
    private List<GameObject> _nearbyBlockList = new List<GameObject>();

    private SM_Block _smBlock;
    public event Action<GameObject> OnBlockBurnt; // "event" when called within class holding the event. argument type already defined by Action<>

    YieldInstruction waitOneSec = new WaitForSeconds(1.0f);
    

    // Start is called before the first frame update
    void Start()
    {
        _smBlock = new SM_Block();
        _smBlock.Initialize();
        _smBlock.CheckComponent();

        _smBlock.BSM_State_Burnt.OnEnter += Burnt;

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
