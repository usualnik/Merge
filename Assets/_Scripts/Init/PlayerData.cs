using System.Collections.Generic;
using UnityEngine;
//using YG;

[System.Serializable]
public class Data
{
    public int Money = 0;
    public int Clicks = 0;
    public int ItemsAvailableInShop = 3;
    public int CurrentItemIndex = 0;
    public List<int> itemsBought = new List<int>();
    public bool IsFirstTimePlaying = true;
    public int EndingsOpened = 0;
    public List<int> AlreadyEndedWithItemIndex = new List<int>();
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
        //if (YG2.saves.YandexServerData != null && YG2.saves.YandexServerData != data)
        //{
        //    LoadPlayerData();
        //}
        //else
        //{
        //    InitFirstTimePlayingData();
        //}
    }

    private void InitFirstTimePlayingData()
    {
        //build new data
        data = new Data();
        data.itemsBought = new List<int>();
    }

    private void LoadPlayerData()
    {
        //this.data = YG2.saves.YandexServerData;              
    }
    private void SavePlayerDataToYandex()
    {
        //YG2.saves.YandexServerData = this.data;
        //YG2.SaveProgress();
    }

    public void HardResetPlayerData()
    {
        data.Money = 0;
        data.Clicks = 0;
        data.ItemsAvailableInShop = 3;
        data.CurrentItemIndex = 0;
        data.itemsBought.Clear();
        data.IsFirstTimePlaying = true;        
    }

    #region Get
    public int GetCurrentMoneyAmount() => data.Money;
    public int GetCurrentClicksAmount() => data.Clicks;
    public int GetItemsAvailableInShop() => data.ItemsAvailableInShop;
    public bool GetIsFirstTimePlaying() => data.IsFirstTimePlaying;
    public List<int> GetItemsBought() => data.itemsBought;
    public int GetCurrentItemIndex() => data.CurrentItemIndex;
    public int GetEndingsOpened() => data.EndingsOpened;
    public List<int> GetAlreadyEndedWithItemIndex() => data.AlreadyEndedWithItemIndex;
    public bool GetTelegramRewardRecived() => data.IsTelegramRewardRecived;
    public bool GetShouldShowAd() => data.ShouldShowAd;


    #endregion

    #region Set

    public void SetCurrentMoney(int value)
    {
        var temp = data.Money;
        data.Money = value;

        if (data.Money < 0)
        {
            data.Money = temp;
        }
        else
        {
            SavePlayerDataToYandex();
        }

    }

    public void SetEndingsOpened(int value)
    {
       data.EndingsOpened = value;

        if (data.EndingsOpened > 10)
        {
            data.EndingsOpened = 10;
        }

        SavePlayerDataToYandex();
    }

    public void AddItemIndexToAlreadeEndedWithItemIndex(int itemIndex)
    {
        data.AlreadyEndedWithItemIndex.Add(itemIndex);
        SavePlayerDataToYandex();
    }
    public void SetCurrentClicks(int value)
    {
        var temp = data.Clicks;
        data.Clicks = value;

        if (data.Clicks < 0)
        {
            data.Clicks = temp;
        }
        else
        {
            SavePlayerDataToYandex();
        }
    }
    public void SetItemsAvailableInShop(int itemsAvailable)
    {
        var temp = data.ItemsAvailableInShop;
        data.ItemsAvailableInShop = itemsAvailable;

        if (data.ItemsAvailableInShop < 0)
        {
            data.ItemsAvailableInShop = temp;
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
    public void SetNewCurrentItemIndex(int index)
    {
        data.CurrentItemIndex = index;
        SavePlayerDataToYandex();
    }

    public void AddNewItemToBoughtList(int index)
    {
        data.itemsBought.Add(index);
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