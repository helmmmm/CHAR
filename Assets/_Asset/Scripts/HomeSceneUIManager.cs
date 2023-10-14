using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using UnityEngine.UI;

public class HomeSceneUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _startButton;
    // Start is called before the first frame update
    void Start()
    {
        _startButton.SetActive(true);
        SM_Scene.Instance.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartButtonClick()
    {
        UnitySceneManager.LoadScene("Game");
        SM_Scene.Instance.TryChangeState(SM_Scene.Instance.SSM_State_GameScene);
    }
}
