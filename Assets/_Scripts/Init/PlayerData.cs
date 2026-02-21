using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;

[System.Serializable]
public class Data
{
    public int Gold = 0;
    public int Score = 0;
    public int Hints = 0;
    public List<int> OpenedBrainrotsIndexes = new List<int>();

    public bool IsFirstTimePlaying = true;

    public bool IsTelegramRewardRecived = false;
    public bool ShouldShowAd = true;
 }

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }
    [SerializeField] private Data data;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameObject.transform.SetParent(null, false);
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        if (YG2.saves.YandexServerData != null && YG2.saves.YandexServerData != data)
        {
            LoadPlayerData();
        }
        else
        {
            InitFirstTimePlayingData();
        }
    }

    private void InitFirstTimePlayingData()
    {
        //build new data
        data = new Data();
        data.OpenedBrainrotsIndexes = new List<int>();
    }

    private void LoadPlayerData()
    {
        this.data = YG2.saves.YandexServerData;
    }
    private void SavePlayerDataToYandex()
    {
        YG2.saves.YandexServerData = this.data;
        YG2.SaveProgress();
    }

    public void HardResetPlayerData()
    {
        data.Gold = 0;
        data.Score = 0;      
        data.Hints = 0;
        data.IsFirstTimePlaying = true;        
    }

    #region Get
    public int GetCurrentGoldAmount() => data.Gold;
    public int GetCurrentHintsAmount() => data.Hints;
    public int GetCurrentScoreAmount() => data.Score;
    public bool GetIsFirstTimePlaying() => data.IsFirstTimePlaying;
    public List<int> GetOpenedBrainrotsIndexes() => data.OpenedBrainrotsIndexes;
    public bool GetTelegramRewardRecived() => data.IsTelegramRewardRecived;
    public bool GetShouldShowAd() => data.ShouldShowAd;


    #endregion

    #region Set

    public void SetCurrentGold(int value)
    {
        var temp = data.Gold;
        data.Gold = value;

        if (data.Gold < 0)
        {
            data.Gold = temp;
        }
        else
        {
            SavePlayerDataToYandex();
        }
    }

    public void ChangeOpenedBrainrotIndexes(HashSet<int> _openedIndexes)
    {
        data.OpenedBrainrotsIndexes = _openedIndexes.ToList();
        SavePlayerDataToYandex();
    }
    public void SetCurrentScore(int value)
    {
        var temp = data.Score;
        data.Score = value;

        if (data.Score < 0)
        {
            data.Score = temp;
        }
        else
        {
            SavePlayerDataToYandex();
        }
    }

    public void SetCurrentHints(int value)
    {
        var temp = data.Hints;
        data.Hints = value;

        if (data.Hints < 0)
        {
            data.Hints = temp;
        }
        else
        {
            SavePlayerDataToYandex();
        }
    }
    public void ChangeFisrtTimePlaying(bool fistTimePlaying)
    {
        data.IsFirstTimePlaying = fistTimePlaying;
        SavePlayerDataToYandex();
    } 
  
    public void SetTelegramRewardRecived(bool recived)
    {
        data.IsTelegramRewardRecived = recived;
        SavePlayerDataToYandex();
    }

    public void SetShouldShowAd(bool shouldShow)
    {
        data.ShouldShowAd = shouldShow;
        SavePlayerDataToYandex();
    }

    #endregion

}