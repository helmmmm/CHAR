using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/*
    * "c" for changing scene
    * "f" for progressing fire on block
    * "t" for tracking plane
    * "p" for pausing and resuming
*/

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    SM_Scene _smScene => SM_Scene.Instance;
    SM_Game _smGame => SM_Game.Instance;
    private float burnTimer = 0;
    private List<GameObject> _generatedBurnables = new List<GameObject>();
    private int _startingFireCount = 5;

    // UI
    private GameObject _fightFireButton;

    void Start()
    {
        Instance = this;
        _smScene.Initialize();
        // _smBlock.Initialize();
        _smGame.Initialize();

        _smGame.GSM_State_CursorPlaced.OnEnter += ActivateLevelGeneration;

        _smGame.GSM_State_LevelGenerated.OnEnter += FillBurnableList;
        
    }

    private void ActivateLevelGeneration()
    {
        // Generate Level
        // UI "generating level"
    }

    private void FillBurnableList()
    {
        _generatedBurnables.Clear();
        GameObject level = GameObject.Find("Level(Clone)");
        foreach (Transform child in level.transform)
        {
            if (child.gameObject.tag == "Burnable")
            {
                _generatedBurnables.Add(child.gameObject);
            }
        }
    }

    public void StartFire()
    {
        StartCoroutine("co_CountDownAndIgnite");
    }

    IEnumerator co_CountDownAndIgnite() // from UI eventsystem
    {
        Debug.Log("starting fire");
        yield return new WaitForSeconds(3);
        IgniteRandomBlocks();
    }

    private void IgniteRandomBlocks()
    {
        for (int i = 0; i < _startingFireCount; i++)
        {   // Make sure to regenerate level if number burnables is less than starting fire count
            int randomIndex = Random.Range(0, _generatedBurnables.Count - 1);
            int blockCount = _generatedBurnables[randomIndex].transform.childCount;
            int randomBlockIndex = Random.Range(0, blockCount - 1);
            Block block = _generatedBurnables[randomIndex].transform.GetChild(randomBlockIndex).GetComponent<Block>();
            block.Ignite();
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
