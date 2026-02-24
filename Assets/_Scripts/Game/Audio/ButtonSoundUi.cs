using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundUi : MonoBehaviour
{    
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    void Start()
    {
        _button.onClick.AddListener(PlayButtonSound);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(PlayButtonSound);
    }

    private void PlayButtonSound()
    {
        AudioManager.Instance.Play("Button");
    }
}
