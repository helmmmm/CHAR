using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
using UnityEngine.UI;

public class GameSceneUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _homeButton;
    [SerializeField] private GameObject _startLevelButton;
    // Start is called before the first frame update
    void Start()
    {
        _homeButton.SetActive(true);
        _startLevelButton.SetActive(false);
        SM_Game.Instance.GSM_State_LevelGenerated.OnEnter += () => _startLevelButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHomeButtonClick()
    {
        UnitySceneManager.LoadScene("Home");
        SM_Scene.Instance.TryChangeState(SM_Scene.Instance.SSM_State_HomeScene);
    }

    public void OnStartLevelButtonClick()
    {
        if (SM_Game.Instance.IsLevelGenerated)
        {
            _startLevelButton.SetActive(false);
            SM_Game.Instance.TryChangeState(SM_Game.Instance.GSM_State_Firefighting);
            GameManager.Instance.StartFire();
        }
    }
}
