using UnityEngine;
using YG;

public class GiveConsumedPayments : MonoBehaviour
{
    private void Start()
    {
       //YG2.onPurchaseSuccess += GiveReward;
    }
    private void OnDestroy()
    {
        //YG2.onPurchaseSuccess -= GiveReward;
    }

    private void GiveReward(string id)
    {
        PlayerData.Instance.SetShouldShowAd(false);
    }
}
