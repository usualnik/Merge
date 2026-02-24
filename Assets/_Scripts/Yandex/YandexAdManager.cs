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
    private const float AD_COOLDOWN_STORE_DELAY = 10f;

    private float adTimer;
    private const string AD_FREE_PURCHASE_ID = "0";

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
        if (!_shouldShowAd) return;

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
        else if (adTimer <= AD_WARNING_TIME)
        {
            // Если открыто окно магазина и таймер подошел к показу рекламы - рестартим таймер
            adTimer = AD_COOLDOWN_STORE_DELAY;
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

    private void OnPurchaseSuccess(string id)
    {
        if (id == AD_FREE_PURCHASE_ID)
        {
            _shouldShowAd = false;
            PlayerData.Instance.SetShouldShowAd(_shouldShowAd);

            YG2.ConsumePurchaseByID(id); // Консумируем покупку

            _adFreeButton.gameObject.SetActive(false);
            _adFreeText.gameObject.SetActive(false);
            _adWarning.SetActive(false);
        }
    }

    public void ShowRewarded()
    {
        YG2.RewardedAdvShow("", () =>
        {
            GameManager.Instance.ReceiveHint(1);
            
        });
    }
}