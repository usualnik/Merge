using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TelegramButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rewardText;
    [SerializeField] private int _rewardAmount = 3;
    private Button _telegramButton;

    private const string TELEGRAM_GROUP_PATH = "https://t.me/ronin_nation";

    private bool _rewardRecived = false;

    private void Awake()
    {
        _telegramButton = GetComponent<Button>();
    }

    private void Start()
    {
        if (_telegramButton != null)
            _telegramButton.onClick.AddListener(OnClick);

        InitButton();
    }
    private void OnDestroy()
    {
        if (_telegramButton != null)
            _telegramButton.onClick.RemoveListener(OnClick);
    }

    private void InitButton()
    {
        if (PlayerData.Instance.GetTelegramRewardRecived())
        {
            _rewardRecived = true;
            gameObject.SetActive(false);
        }
    }

    private void OnClick()
    {
        if (_rewardRecived)
        {
            Application.OpenURL(TELEGRAM_GROUP_PATH);
            return;
        }

        Application.OpenURL(TELEGRAM_GROUP_PATH);
        GameManager.Instance.ReceiveHint(_rewardAmount);

        _rewardText.gameObject.SetActive(false);

        _rewardRecived = true;
        PlayerData.Instance.SetTelegramRewardRecived(_rewardRecived);
    }
}
