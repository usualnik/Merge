using TMPro;
using UnityEngine;
using YG;

public class UI_AdWarningText : MonoBehaviour
{
    public bool IsTimerRunning => _isTimerRunnnig;

    private TextMeshProUGUI _adWarningtext;
    private const float AD_WARNING_TIMER_MAX = 2f;
    private float _adWarningTimer;

    private bool _isTimerRunnnig = false;

    private void Awake()
    {      

        _adWarningtext = GetComponentInChildren<TextMeshProUGUI>();
        _adWarningTimer = AD_WARNING_TIMER_MAX;
    }

    private void OnEnable()
    {
        _isTimerRunnnig = true;
        _adWarningTimer = AD_WARNING_TIMER_MAX;
    }

    private void OnDisable()
    {
        _isTimerRunnnig = false;
        _adWarningTimer = AD_WARNING_TIMER_MAX;
    }

    private void Update()
    {
        if (!_isTimerRunnnig)
            return;

        _adWarningTimer -= Time.deltaTime;

        if (_adWarningTimer > 1)
        {
            _adWarningtext.text = YG2.envir.language == "ru"
                ? string.Format($"Реклама через... {_adWarningTimer.ToString("0")}")
                : string.Format($"Ad starts in ... {_adWarningTimer.ToString("0")}");
        }        
    }
}