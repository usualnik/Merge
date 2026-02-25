using UnityEngine;
using YG;

public class GiveConsumedPayments : MonoBehaviour
{
    //ID's
    private const string NO_ADS_ID = "0";
    private const string SMALL_MONEY_PACK_ID = "1";
    private const string MEDIUM_MONEY_PACK_ID = "2";
    private const string BIG_MONEY_PACK_ID = "3";

    //REWARDS
    private const int SMALL_MONEY_PACK = 200;
    private const int MEDIUM_MONEY_PACK = 800;
    private const int BIG_MONEY_PACK = 1500;


    private void Start()
    {
        YG2.onPurchaseSuccess += GiveReward;
    }
    private void OnDestroy()
    {
        YG2.onPurchaseSuccess -= GiveReward;
    }

    private void GiveReward(string id)
    {
        switch (id)
        {
            case NO_ADS_ID:
                PlayerData.Instance.SetShouldShowAd(false);
                break;
            case SMALL_MONEY_PACK_ID:
                PlayerData.Instance.SetCurrentGold(PlayerData.Instance.GetCurrentGoldAmount() 
                    + SMALL_MONEY_PACK);
                break;
            case MEDIUM_MONEY_PACK_ID:
                PlayerData.Instance.SetCurrentGold(PlayerData.Instance.GetCurrentGoldAmount() 
                    + MEDIUM_MONEY_PACK);
                break;
            case BIG_MONEY_PACK_ID:
                PlayerData.Instance.SetCurrentGold(PlayerData.Instance.GetCurrentGoldAmount() 
                    + BIG_MONEY_PACK);
                break;
            default:
                break;
        }
    }
}
