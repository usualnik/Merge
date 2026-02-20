using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class IngredientDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool IsInMixingZone => _isAtMixingZone;

    private bool _isAtMixingZone = false;
    private bool _isOriginal = true;

    protected bool _isDragging;
    protected Canvas _canvas;
    private GameObject _dragObject;

    private BoxCollider2D _boxCollider2D;

    private const string MIXING_PANEL_TAG = "MixingPanel";

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
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isDragging) return;
        if (_dragObject != null) return;


        _isDragging = true;

        if (_isOriginal)
        {
            _dragObject = Instantiate(gameObject, gameObject.transform.position,
                gameObject.transform.rotation, _canvas.transform);

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

        if (!_dragObject.GetComponent<IngredientDrag>().IsInMixingZone)
        {
            Destroy(_dragObject);
            _dragObject = null;
        }

        _isDragging = false;

        if (_dragObject)
            PlaceItemInMixingSpace();

    }

    private void PlaceItemInMixingSpace()
    {
        MixingManager.Instance.PutItemInMixingZone(_dragObject.GetComponent<Ingredient>());
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