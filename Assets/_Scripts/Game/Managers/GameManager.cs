using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action<RecepieDataSO> OnNewRecepieFound;

    [Header("Visual")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _hintsText;

    [Header("Pause")]
    [SerializeField] private GameObject _pausePanel;


    [Header("Game values")]
    [SerializeField] private int _score;
    [SerializeField] private int _coins;
    [SerializeField] private int _hints;
    [SerializeField] private HashSet<int> _openedBrainrotIndexes;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameObject.transform.SetParent(null, false);
        }
        else
            Destroy(gameObject);

        _openedBrainrotIndexes = new HashSet<int>();
    }

    private void Start()
    {
        MixingManager.Instance.OnRecepieFound += MixingManager_OnRecepieFound;

        LoadGameData();
    }
    private void OnDestroy()
    {
        MixingManager.Instance.OnRecepieFound -= MixingManager_OnRecepieFound;

    }

    private void LoadGameData()
    {
        _coins = PlayerData.Instance.GetCurrentGoldAmount();
        _hints = PlayerData.Instance.GetCurrentHintsAmount();
        _score = PlayerData.Instance.GetCurrentScoreAmount();

        _coinsText.text = _coins.ToString();
        _hintsText.text = _hints.ToString();
        _scoreText.text = _score.ToString();

        _openedBrainrotIndexes = PlayerData.Instance.GetOpenedBrainrotsIndexes().ToHashSet();
    }

    private void MixingManager_OnRecepieFound(RecepieDataSO obj)
    {
        HandleRecepieFound(obj);
    }

    private void HandleRecepieFound(RecepieDataSO recepieFoundData)
    {
        if (recepieFoundData == null || _openedBrainrotIndexes.Contains(recepieFoundData.RecepieID))
            return;

        _score++;
        _scoreText.text = _score.ToString();

        _openedBrainrotIndexes.Add(recepieFoundData.RecepieID);
        PlayerData.Instance.ChangeOpenedBrainrotIndexes(_openedBrainrotIndexes);                       
       
        OnNewRecepieFound?.Invoke(recepieFoundData);
    }

    public void PauseGame()
    {
        _pausePanel.gameObject.SetActive(true);
    }

    public void UnPauseGame()
    {
        _pausePanel.gameObject.SetActive(false);
    }

    public void ReceiveHint()
    {
        _hints++;
        _hintsText.text = _hints.ToString();

        PlayerData.Instance.SetCurrentHints(PlayerData.Instance.GetCurrentHintsAmount() + 1);
    }

    public int Score => _score;
    public int Coins => _coins;
    public int Hints => _hints;
    public HashSet<int> RecepieFoundIndexes => _openedBrainrotIndexes;
}
