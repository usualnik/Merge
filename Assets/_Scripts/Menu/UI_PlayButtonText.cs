using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using YG;

public class UI_PlayButtonText : MonoBehaviour
{
    private Button _loadGameButton;
    private TextMeshProUGUI _loadGameButtonText; 

    private string _playButtonTextRU = "«¿√–”« ¿...";
    private string _playButtonTextEN = "LOADING...";


    private void Awake()
    {
        _loadGameButton = GetComponent<Button>();
        _loadGameButtonText = GetComponentInChildren<TextMeshProUGUI>();
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
        _loadGameButtonText.text = /*YG2.envir.language == "ru"*/ true ? _playButtonTextRU : _playButtonTextEN;
    }
}
