using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class IngredientDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public bool IsInMixingZone => _isAtMixingZone;

    private Transform _mixingZoneTransform;

    private bool _isAtMixingZone = false;
    private bool _isOriginal = true;

    protected bool _isDragging;
    protected Canvas _canvas;
    private GameObject _dragObject;
    private CenterTransform _centerTransform;

    private BoxCollider2D _boxCollider2D;

    private const string MIXING_PANEL_TAG = "MixingPanel";

    // Параметры для спирального размещения
    [Header("Spiral Settings")]
    [SerializeField] private float spiralRadius = 50f;
    [SerializeField] private float spiralStep = 30f;
    [SerializeField] private int maxObjectsInCircle = 8;

    // Статический список для отслеживания всех объектов в центре
    private static List<GameObject> objectsInCenter = new List<GameObject>();

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        _canvas = GetComponentInParent<Canvas>();

        if (_canvas == null)
        {
            Debug.LogError("Canvas not found in parent hierarchy!", this);
        }

        _centerTransform = FindAnyObjectByType<CenterTransform>();
        _mixingZoneTransform = FindAnyObjectByType<MixingPanel>().gameObject.transform;
    }

    private void OnDestroy()
    {
        // Удаляем объект из списка при уничтожении
        if (objectsInCenter.Contains(gameObject))
        {
            objectsInCenter.Remove(gameObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isDragging) return;
        if (_dragObject != null) return;

        GameManager.Instance.IsSomethingGoingOn = true;
        _isDragging = true;

        if (_isOriginal)
        {
            _dragObject = Instantiate(gameObject, gameObject.transform.position,
                gameObject.transform.rotation, _mixingZoneTransform);

            if (_dragObject.GetComponentInChildren<IngredientCellVisual>() != null)
            {
                _dragObject.GetComponentInChildren<IngredientCellVisual>().SetVisualActive(false);
            }

            if (_dragObject.TryGetComponent(out IngredientDrag ingredientDrag))
            {
                ingredientDrag.SetIsOriginal(false);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging) return;
        if (_dragObject == null) return;

        if (_isOriginal)
        {
            _dragObject.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isDragging) return;
        if (_dragObject == null) return;

        GameManager.Instance.IsSomethingGoingOn = false;


        if (!_dragObject.GetComponent<IngredientDrag>().IsInMixingZone)
        {
            Destroy(_dragObject);
            _dragObject = null;
        }

        _isDragging = false;

        if (_dragObject)
        {
            PlaceItemInMixingSpace();
            // Добавляем объект в список после успешного размещения
            if (!objectsInCenter.Contains(_dragObject))
            {
                objectsInCenter.Add(_dragObject);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isOriginal) return;
        if (_isDragging) return;

        SpawnAtCenter();
    }

    private void SpawnAtCenter()
    {
        if (_centerTransform == null)
        {
            Debug.LogError("CenterTransform not found!", this);
            return;
        }

        // Создаем объект
        _dragObject = Instantiate(gameObject,
            _centerTransform.transform.position,
            Quaternion.identity,
            _mixingZoneTransform);

        if (_dragObject.GetComponentInChildren<IngredientCellVisual>() != null)
        {
            _dragObject.GetComponentInChildren<IngredientCellVisual>().SetVisualActive(false);
        }

        if (_dragObject.TryGetComponent(out IngredientDrag ingredientDrag))
        {
            ingredientDrag.SetIsOriginal(false);
        }

        // Рассчитываем позицию по спирали
        Vector3 spiralPosition = CalculateSpiralPosition();
        _dragObject.transform.localPosition = spiralPosition;

        // Добавляем в список и размещаем
        objectsInCenter.Add(_dragObject);
        PlaceItemInMixingSpace();

        _dragObject = null;
    }

    private Vector3 CalculateSpiralPosition()
    {
        int currentCount = objectsInCenter.Count;

        // Если это первый объект - оставляем в центре
        if (currentCount == 0)
        {
            return Vector3.zero;
        }

        // Определяем круг и позицию в круге
        int circleIndex = currentCount / maxObjectsInCircle;
        int positionInCircle = currentCount % maxObjectsInCircle;

        // Радиус увеличивается с каждым кругом
        float radius = spiralRadius * (circleIndex + 1);

        // Угол для равномерного распределения по кругу
        float angle = (360f / maxObjectsInCircle) * positionInCircle * Mathf.Deg2Rad;

        // Рассчитываем позицию
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        // Добавляем небольшое смещение по вертикали для создания спирали
        float spiralOffset = circleIndex * spiralStep;
        y += spiralOffset;

        return new Vector3(x, y, 0);
    }

    private void PlaceItemInMixingSpace()
    {
        if (_dragObject != null)
        {
            RecepiesManager.Instance.PutItemInMixingZone(_dragObject.GetComponent<Ingredient>());
        }
    }

    public void SetIsOriginal(bool canBeDragged)
    {
        _isOriginal = canBeDragged;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == MIXING_PANEL_TAG)
        {
            _isAtMixingZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == MIXING_PANEL_TAG)
        {
            _isAtMixingZone = false;
        }
    }


}