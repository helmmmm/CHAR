using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour
{
    [SerializeField] private TMP_Text _gameTimerText;
    public static CountDown Instance;
    private float _timeDuration = 60f;
    private float _timer;
    public bool _timerGoing = true;
    // public float _currentTime;
    // private float _minLeft;
    // private float _secLeft;

    void Start()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (_timerGoing)
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                UpdateTimerDisplay(_timer);
            }
            
            if (_timer <= 0 && _timerGoing)
            {
                UpdateTimerDisplay(_timer);
                _timerGoing = false;
                SM_Game.Instance.TryChangeState(SM_Game.Instance.GSM_State_GameFinished);
            }

            Debug.Log(GameManager.Instance._currentFireCount);
            if (GameManager.Instance.IsFireGone())
            {
                _timerGoing = false;
                ScoreManager.Instance._timeLeft = Mathf.FloorToInt(_timer);
                SM_Game.Instance.TryChangeState(SM_Game.Instance.GSM_State_GameFinished);
            }
        }
    }


    private void UpdateTimerDisplay(float time)
    {
        float min = Mathf.FloorToInt(_timer / 60);
        float sec = Mathf.FloorToInt(_timer % 60);

        string currentTime = string.Format("{0}:{1:00}", min, sec); 
        _gameTimerText.text = currentTime;  
    }

    // Stop timer


    // Upload time left to score manager when game ends
    private void ResetTimer()
    {
        _timer = _timeDuration;
    }
}
