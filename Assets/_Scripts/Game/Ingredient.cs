using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Ingredient : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public IngredientDataSO IngredientData => _ingredientData;

    [SerializeField] private IngredientDataSO _ingredientData;
    [SerializeField] private float _pulseScale = 1.2f;
    [SerializeField] private float _pulseDuration = 0.3f;

    private IngredientVisual ingredientImage;
    private TextMeshProUGUI _ingredientName;
    private Sequence _pulseSequence; 

    private void Awake()
    {
        _ingredientName = GetComponentInChildren<TextMeshProUGUI>();
        ingredientImage = GetComponentInChildren<IngredientVisual>(); 
    }

    public void InitIngredient(IngredientDataSO data)
    {
        _ingredientData = data;
        _ingredientName.text = _ingredientData.IngredientName;
        ingredientImage.Visual.sprite = data.IngredientSprite;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

 
    public void StartPulsing()
    {
        StopPulsing();

        _pulseSequence = DOTween.Sequence();

        _pulseSequence.Append(transform.DOScale(_pulseScale, _pulseDuration).SetEase(Ease.InOutSine));
        _pulseSequence.Append(transform.DOScale(1f, _pulseDuration).SetEase(Ease.InOutSine));
        _pulseSequence.SetLoops(-1);
        _pulseSequence.Play();
    }

    public void StopPulsing()
    {
        if (_pulseSequence != null && _pulseSequence.IsActive())
        {
            _pulseSequence.Kill();
            _pulseSequence = null;
        }

        transform.localScale = Vector3.one;
    }


    private void OnDestroy()
    {
        // Очищаем твины при уничтожении объекта
        StopPulsing();
        transform.DOKill();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StopPulsing();
    }

    public void OnDrag(PointerEventData eventData)
    {
        StopPulsing();
    }
}