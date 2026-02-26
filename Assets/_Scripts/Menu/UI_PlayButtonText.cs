using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class UI_PlayButtonText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _loadGameButtonText;

    private Button _loadGameButton;

    private string _playButtonTextRU = "«¿√–”« ¿...";
    private string _playButtonTextEN = "LOADING...";


    private void Awake()
    {
        _loadGameButton = GetComponent<Button>();
    }

    private void Start()
    {
        _loadGameButton.onClick.AddListener(ChangePlayButtonText);
    }
    private void OnDestroy()
    {
        _loadGameButton.onClick.RemoveListener(ChangePlayButtonText);

    }

    private void ChangePlayButtonText()
    {
        _loadGameButtonText.text = YG2.envir.language == "ru" ? _playButtonTextRU : _playButtonTextEN;
    }
}
