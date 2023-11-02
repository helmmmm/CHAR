using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    GameSceneUIManager _gsum => GameSceneUIManager.Instance;

    public int _totalBlockCount; // From GameManager [v]
    public int _burntBlockCount; // From Block Class [v]
    public int _ignitedBlockCount; // From Block Class [v]
    public int _extinguishedBlockCount; // From Block Class [v]
    public int _totalWaterCount;// From WaterShooter [v]
    public int _hitWaterCount; // From Block Class [v]
    // public int _timeLeft; // From CountDown []

    private int _burntPenaltyScore;
    private int _extinguishedBonusScore;
    private int _waterAccuracyBonusScore;
    private int _overallScore;
    
    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ResetScore();
    }

    private void OnEnable() 
    {
        SM_Game.Instance.GSM_State_GameFinished.OnEnter += RunScoreRoutine;
        SM_Game.Instance.GSM_State_GameFinished.OnExit += ResetScore;
    }

    private void OnDisable() 
    {
        SM_Game.Instance.GSM_State_GameFinished.OnEnter -= RunScoreRoutine;
        SM_Game.Instance.GSM_State_GameFinished.OnExit -= ResetScore;
    }

    
    void Update()
    {
        
    }

    // private void DisplayScore()
    // {

    // }

    // private void SaveScore()
    // {

    // }

    private void RunScoreRoutine()
    {
        StartCoroutine(ScoreRoutine());
    }

    IEnumerator ScoreRoutine()
    {
        CalculateScore();
        SaveScore();
        UpdateScoreText();
        DisplayScore();

        yield return null;
    }

    private void CalculateScore()
    {
        _burntPenaltyScore = Mathf.RoundToInt(BurntBlockPercentage() * 200);
        _extinguishedBonusScore = Mathf.RoundToInt(ExtinguishedBlockPercentage() * 200);
        _waterAccuracyBonusScore = Mathf.RoundToInt(WaterAccuracy() * 400);

        _overallScore = _extinguishedBonusScore + _waterAccuracyBonusScore - _burntPenaltyScore;
    }

    private void SaveScore()
    {
        // Save score to database
    }

    private void UpdateScoreText()
    {
        _gsum.ChangeText(_gsum._burntRaw, FormatPercentage(BurntBlockPercentage()));
        _gsum.ChangeText(_gsum._extinguishedRaw, FormatPercentage(ExtinguishedBlockPercentage()));
        _gsum.ChangeText(_gsum._waterAccuracyRaw, FormatPercentage(WaterAccuracy()));

        _gsum.ChangeText(_gsum._burntPenalty, ("-" + _burntPenaltyScore.ToString()));
        _gsum.ChangeText(_gsum._extinguishedBonus, (_extinguishedBonusScore.ToString()));
        _gsum.ChangeText(_gsum._waterAccuracyBonus, (_waterAccuracyBonusScore.ToString()));

        _gsum.ChangeText(_gsum._overallScore, _overallScore.ToString());
    }

    private void DisplayScore()
    {
        _gsum.EnableFinishScreen();
    }

    private string FormatPercentage(float decimalValue)
    {
        string percentage = decimalValue.ToString("P0");
        return percentage;
    }

    public float BurntBlockPercentage()
    {
        // Debug.Log(_burntBlockCount + "\n" + _totalBlockCount + "\n");
        // Debug.Log((float)_burntBlockCount / (float)_totalBlockCount + "\n");
        return (float)_burntBlockCount / (float)_totalBlockCount;
    }

    public float ExtinguishedBlockPercentage()
    {
        Debug.Log(_extinguishedBlockCount + "\n" + _ignitedBlockCount + "\n");
        return (float)_extinguishedBlockCount / (float)_ignitedBlockCount;
    }

    public float WaterAccuracy()
    {
        Debug.Log(_hitWaterCount + "\n" + _totalWaterCount + "\n");
        return (float)_hitWaterCount / (float)_totalWaterCount;
    }

    // public float TimeLeftPercentage()
    // {
    //     return (float)_timeLeft / _timeDuration;
    // }

    public void ResetScore()
    {
        _totalBlockCount = 0;
        _burntBlockCount = 0;
        _ignitedBlockCount = 0;
        _extinguishedBlockCount = 0;
        _totalWaterCount = 0;
        _hitWaterCount = 0;
        // _timeLeft = 0;
    }

    private void OnDestroy() 
    {
        
    }
}
