using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Magazine")]
    [SerializeField] private GameObject _magazinePanel;

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
        RecepiesManager.Instance.OnRecepieFound += MixingManager_OnRecepieFound;


        LoadGameData();
    }
    private void OnDestroy()
    {
        RecepiesManager.Instance.OnRecepieFound -= MixingManager_OnRecepieFound;

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
        PlayerData.Instance.SetCurrentScore(PlayerData.Instance.GetCurrentScoreAmount() + _score);

        GivePlayerRewardForFinding(recepieFoundData);

        OnNewRecepieFound?.Invoke(recepieFoundData);
    }

    private void GivePlayerRewardForFinding(RecepieDataSO recepieFoundData)
    {

        switch (recepieFoundData.RecepieRarity)
        {
            case RecepieDataSO.Rarity.Common:
                ChangeCoins(2);
                AudioManager.Instance.PlayWithPitch("Success", 1.0f);
                break;
            case RecepieDataSO.Rarity.Rare:
                ChangeCoins(4);
                AudioManager.Instance.PlayWithPitch("Success", 1.2f);
                break;
            case RecepieDataSO.Rarity.Legendary:
                ChangeCoins(8);
                AudioManager.Instance.PlayWithPitch("Success", 1.4f);
                break;
            case RecepieDataSO.Rarity.Mythic:
                ChangeCoins(16);
                AudioManager.Instance.PlayWithPitch("Success", 1.6f);
                break;
            case RecepieDataSO.Rarity.BrainrotGod:
                ChangeCoins(32);
                AudioManager.Instance.PlayWithPitch("Success", 1.8f);
                break;
        }


    }

    public void PauseGame()
    {
        _pausePanel.gameObject.SetActive(true);
    }

    public void UnPauseGame()
    {
        _pausePanel.gameObject.SetActive(false);
    }


    public void BuyHint()
    {
        if (_coins <= 10) return;

        ChangeCoins(-10);
        ReceiveHint(1);
    }

    public void ReceiveHint(int value)
    {
        AudioManager.Instance.Play("HintReceived");

        _hints += value;
        _hintsText.text = _hints.ToString();

        PlayerData.Instance.SetCurrentHints(PlayerData.Instance.GetCurrentHintsAmount() + value);
    }

    public void GiveHint()
    {
        AudioManager.Instance.Play("HintUsed");

        _hints--;
        _hintsText.text = _hints.ToString();
        PlayerData.Instance.SetCurrentHints(PlayerData.Instance.GetCurrentHintsAmount() - 1);
    }

    public void ChangeCoins(int value)
    {
        AudioManager.Instance.Play("Coins");

        var temp = _coins;
        _coins += value;

        if (_coins < 0)
        {
            _coins = temp;
        }

        PlayerData.Instance.SetCurrentGold(_coins);
        _coinsText.text = _coins.ToString();
    }

    public void OpenMagazinePanel()
    {
        _magazinePanel.SetActive(true);
    }
    public void HideMagazinePanel()
    {
        _magazinePanel.SetActive(false);
    }

    public void AddScore(int value)
    {
        _score += value;

        PlayerData.Instance.SetCurrentScore(_score);
        _scoreText.text = _score.ToString();
    }

    public int Score => _score;
    public int Coins => _coins;
    public int Hints => _hints;
    public HashSet<int> RecepieFoundIndexes => _openedBrainrotIndexes;
}
