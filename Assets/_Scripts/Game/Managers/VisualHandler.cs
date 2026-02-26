using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class VisualHandler : MonoBehaviour
{
    public static VisualHandler Instance { get; private set; }

    [SerializeField] private RectTransform _animFinalPos;
    [SerializeField] private GameObject _blankVisual;
    [SerializeField] private RectTransform _startPos;


    [Header("Animation Settings")]
    [SerializeField] private float _moveDuration = 1.5f;
    [SerializeField] private float _punchScaleDuration = 0.5f;
    [SerializeField] private float _punchScaleStrength = 0.3f;
    [SerializeField] private Ease _moveEase = Ease.OutQuad;

    private Image _blankImage;
    private RectTransform _blankRectTransform;
    private Canvas _parentCanvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null, false);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnNewRecepieFound += GameManager_OnRecepieFound;
     
        if (_blankVisual != null)
        {
            _blankImage = _blankVisual.GetComponentInChildren<Image>();
            _blankRectTransform = _blankVisual.GetComponent<RectTransform>();

            _parentCanvas = GetComponentInParent<Canvas>();

            _blankVisual.SetActive(false);
        }
        else
        {
            Debug.LogError("_blankVisual is not assigned!");
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnNewRecepieFound -= GameManager_OnRecepieFound;
    

        DOTween.Kill(_blankRectTransform);
        DOTween.Kill(_blankImage);
    }

    private void GameManager_OnRecepieFound(RecepieDataSO obj)
    {
        PlayFoundAnimation(obj);
    }

    private void PlayFoundAnimation(RecepieDataSO recepieFound)
    {
        if (recepieFound == null || recepieFound.RecepieSprite == null)
        {
            Debug.LogWarning("RecepieDataSO or its sprite is null!");
            return;
        }

        // Останавливаем предыдущие анимации
        DOTween.Kill(_blankRectTransform);
        DOTween.Kill(_blankImage);

        
        _blankRectTransform.position = _startPos.transform.position;
       

        // Настраиваем и показываем визуал
        _blankVisual.SetActive(true);
        _blankImage.sprite = recepieFound.RecepieSprite;

        // Сбрасываем масштаб перед анимацией
        _blankRectTransform.localScale = Vector3.one;

        // Создаем последовательность анимаций
        Sequence animationSequence = DOTween.Sequence();

        // Анимация движения к книге рецептов
        Tween moveTween = _blankRectTransform.DOMove(_animFinalPos.position, _moveDuration, true)
            .SetEase(_moveEase);

        // Анимация пульсации
        Tween punchTween = _blankRectTransform.DOPunchScale(
            Vector3.one * _punchScaleStrength,
            _punchScaleDuration,
            vibrato: 5,
            elasticity: 0.5f
        );

        // Добавляем анимации в последовательность
        animationSequence.Append(moveTween);
        animationSequence.Join(punchTween);


        animationSequence.OnComplete(() =>
        {
          
        });

        // Запускаем анимацию
        animationSequence.Play();
    }
}