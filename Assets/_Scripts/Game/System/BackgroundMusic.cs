using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance { get; private set; }

    private AudioSource _audioSource;

    private bool _isPlaying;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gameObject.transform.SetParent(null);


        DontDestroyOnLoad(gameObject);


        _audioSource = GetComponent<AudioSource>();

        _isPlaying = true;

    }

    public void ToggleMute()
    {

        if (_isPlaying)
        {
            _isPlaying = false;
            _audioSource.mute = true;
        }
        else
        {
            _isPlaying = true;
            _audioSource.mute = false;
        }
    }

    public void SetBackgroundMusic(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }

    public bool IsPlaying() => _isPlaying;
}
