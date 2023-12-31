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
    protected float _minTemperature = 20f; // Minimum temperature of the voxel
    protected float _ignitionTemperature = 140f; // Temperature at which it ignites
    protected float _heatTransferRate = 20f; // How much heat it emits
    protected float _burnHP = 220f; // Block's HP against fire
    protected float _burnDMG = 20f; // Damage per second from fire
    protected float _coolingRate = 30f; // How much heat it loses per second

    public bool manualIgnition = false;
    private bool _beenBurnt = false;

    private Collider _collider;
    public List<GameObject> _nearbyBlockList = new List<GameObject>();

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
        // _smBlock.CheckComponent();

        _smBlock.BSM_State_Resting.OnEnter += OnResting;

        _smBlock.BSM_State_Burning.OnEnter += OnBurning;

        _smBlock.BSM_State_Burning.OnExit += DespawnFireVFX;

        _smBlock.BSM_State_Burnt.OnEnter += Burnt;

        _collider = GetComponent<Collider>();

        StartCoroutine(co_EmitHeat());
        StartCoroutine(co_Burn());
    }

    private void OnEnable() 
    {
        
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (manualIgnition)
    //     {
    //         _smBlock.TryChangeState(_smBlock.BSM_State_Burning);
    //         _currentTemperature = _ignitionTemperature;
    //         manualIgnition = false;
    //     }
    // }

    // private void IncreaseBurningCount()
    // {
    //     GameManager.Instance._currentFireCount++;
    // }

    // private void DecreaseBurningCount()
    // {
    //     GameManager.Instance._currentFireCount--;
    //     if (GameManager.Instance.IsFireGone())
    //     {
    //         SM_Game.Instance.TryChangeState(SM_Game.Instance.GSM_State_GameFinished);
    //     }
    // }

    public void Ignite()
    {
        _smBlock.TryChangeState(_smBlock.BSM_State_Burning);
        _currentTemperature = _ignitionTemperature;
    }

    private void OnBurning()
    {
        // if (!_beenBurnt)
        // {
        //     _beenBurnt = true;
        //     ScoreManager.Instance._burntBlockCount++;
        // }
        ToBurningMaterial();
        SpawnFireVFX();
        ScoreManager.Instance._ignitedBlockCount++;
        gameObject.tag = "Burning Block";
        GameManager.Instance._currentFireCount++;
    }

    private void OnResting()
    {
        ToDefaultMaterial();
        DespawnFireVFX();
        gameObject.tag = "Burnable Block";
        GameManager.Instance._currentFireCount--;
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Burnable Block")
        {
            _nearbyBlockList.Add(other.gameObject);
            other.gameObject.GetComponent<Block>().OnBlockBurnt += RemoveFromNearbyBlockList;
        }

        if (other.gameObject.tag == "Water")
        {
            Debug.Log($"Hit with {other.gameObject.name}");
            TryCoolDown(_coolingRate);
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Water VFX")
        {
            Destroy(other.gameObject);
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

        if (UnityEngine.Random.Range(0, 100) > 65) // 35% chance to not gain heat
        {
            return;
        }

        if (Mathf.Abs(heatSourcePosY - transform.position.y) < 0.01f)
        {
            heatFactor = 1f;
        }
        else if (heatSourcePosY > transform.position.y)
        {
            heatFactor = 0.7f;
        }
        else if (heatSourcePosY < transform.position.y)
        {
            heatFactor = 1.6f;
        }

        _currentTemperature += _heatTransferRate * heatFactor;
            
        if (_currentTemperature >= _ignitionTemperature)
        {
            _smBlock.TryChangeState(_smBlock.BSM_State_Burning);
        }
    }

    public void TryCoolDown(float coolingRate)
    {
        if (_currentTemperature <= _minTemperature)
            return;

        if (_smBlock.IsBurningState)
        {
            _currentTemperature -= coolingRate;

            if (_currentTemperature <= _ignitionTemperature)
                _smBlock.TryChangeState(_smBlock.BSM_State_Resting);
                ScoreManager.Instance._extinguishedBlockCount++;
        }
        else
            _currentTemperature -= coolingRate * 0.5f;

        Debug.Log($"[{gameObject.name}] Temperature: {_currentTemperature}\n");
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
            // Debug.Log($"[{gameObject.name}] HP: {_burnHP}\n");

            if (_burnHP <= 0)
            {
                _smBlock.TryChangeState(_smBlock.BSM_State_Burnt);
            }
        }
    }

    void ToBurningMaterial()
    {
        rend.material = _burningMaterial;
    }

    void ToDefaultMaterial()
    {
        rend.material = _defaultMaterial;
    }

    void SpawnFireVFX()
    {
        // if (contact list has 6 || has 5 and the bottom is open)
        if (_nearbyBlockList.Count >= 6)
            return;
        
        if (_nearbyBlockList.Count < 6)
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

    void DespawnFireVFX()
    {
        if (_fireVFXInstance != null)
            Destroy(_fireVFXInstance);
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
        // Debug.Log($"[{gameObject.name}] is Burnt");
        ScoreManager.Instance._burntBlockCount++;
        GameManager.Instance._currentFireCount--;
        gameObject.tag = "Burnt Block";
        OnBlockBurnt?.Invoke(gameObject); // ? to check if OnBlockBurnt is null
        _smBlock.BSM_State_Resting.OnEnter -= OnResting;

        _smBlock.BSM_State_Burning.OnEnter -= OnBurning;

        _smBlock.BSM_State_Burning.OnExit -= DespawnFireVFX;
        // _smBlock.BSM_State_Burning.OnEnter -= DecreaseBurningCount;

        _smBlock.BSM_State_Burnt.OnEnter -= Burnt;   
        Destroy(gameObject);
    }
}
