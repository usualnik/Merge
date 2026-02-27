using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class YandexAdManager : MonoBehaviour
{
    [SerializeField] private GameObject _adWarning;
    [SerializeField] private Button _adFreeButton;
    [SerializeField] private TextMeshProUGUI _adFreeText;

    private bool _shouldShowAd = true;

    private const float AD_COOLDOWN_MAX = 65f;
    private const float AD_WARNING_TIME = 2f;

    private float adTimer;

    //ID's
    private const string AD_FREE_PURCHASE_ID = "0";

    private const string SMALL_MONEY_PACK_ID = "1";
    private const string MEDIUM_MONEY_PACK_ID = "2";
    private const string BIG_MONEY_PACK_ID = "3";

    //REWARDS
    private const int SMALL_MONEY_PACK = 200;
    private const int MEDIUM_MONEY_PACK = 800;
    private const int BIG_MONEY_PACK = 1500;

    private void Awake()
    {
        adTimer = AD_COOLDOWN_MAX;
    }

    private void Start()
    {
        YG2.onPurchaseSuccess += OnPurchaseSuccess;
        _adFreeButton.onClick.AddListener(BuyAdFreePurchase);

        // Просто читаем сохраненное значение
        _shouldShowAd = PlayerData.Instance.GetShouldShowAd();

        if (_shouldShowAd)
        {
            // Если нужно показывать рекламу - показываем кнопку покупки
            _adFreeButton.gameObject.SetActive(true);
            _adFreeText.gameObject.SetActive(true);
        }
        else
        {
            // Если реклама отключена - скрываем кнопку и предупреждение
            _adFreeButton.gameObject.SetActive(false);
            _adFreeText.gameObject.SetActive(false);
            _adWarning.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        _adFreeButton.onClick.RemoveListener(BuyAdFreePurchase);
        YG2.onPurchaseSuccess -= OnPurchaseSuccess;
    }

    private void Update()
    {
        if (!_shouldShowAd /*|| GameManager.Instance.IsSomethingGoiungOn*/) return;

        adTimer -= Time.deltaTime;

        if (adTimer <= AD_WARNING_TIME)
        {
            ShowAdWarning();

            if (adTimer <= 0)
            {
                ShowInterstitial();
                adTimer = AD_COOLDOWN_MAX;
                HideAdWarning();
            }
        }

    }

    private void ShowInterstitial()
    {
        YG2.InterstitialAdvShow();
    }

    private void ShowAdWarning()
    {
        _adWarning.SetActive(true);
    }

    private void HideAdWarning()
    {
        _adWarning.SetActive(false);
    }

    private void BuyAdFreePurchase()
    {
        YG2.BuyPayments(AD_FREE_PURCHASE_ID);
    }

    public void BuyMoneyPack(string moneyPackID)
    {
        YG2.BuyPayments(moneyPackID);
    }

    private void OnPurchaseSuccess(string id)
    {
        switch (id)
        {
            case AD_FREE_PURCHASE_ID:
                HandleDisbleAds(id);
                break;
            case SMALL_MONEY_PACK_ID:
                GameManager.Instance.ChangeCoins(SMALL_MONEY_PACK);
                break;
            case MEDIUM_MONEY_PACK_ID:
                GameManager.Instance.ChangeCoins(MEDIUM_MONEY_PACK);
                break;
            case BIG_MONEY_PACK_ID:
                GameManager.Instance.ChangeCoins(BIG_MONEY_PACK);
                break;
            default:
                break;
        }

        YG2.ConsumePurchaseByID(id);
    }

    private void HandleDisbleAds(string id)
    {
        _shouldShowAd = false;
        PlayerData.Instance.SetShouldShowAd(_shouldShowAd);

        _adFreeButton.gameObject.SetActive(false);
        _adFreeText.gameObject.SetActive(false);
        _adWarning.SetActive(false);
    }

    public void ShowRewarded()
    {
        YG2.RewardedAdvShow("", () =>
        {
            GameManager.Instance.ReceiveHint(1);

        });
    }
}