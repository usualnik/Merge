using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Ingredient : MonoBehaviour
{
    public IngredientDataSO IngredientData => _ingredientData;

    [SerializeField] private IngredientDataSO _ingredientData;

    private Image _visual;
    private TextMeshProUGUI _ingredientName;

    private void Awake()
    {
        _ingredientName = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void InitIngredient(IngredientDataSO data)
    {
        _ingredientData = data;
        _ingredientName.text = _ingredientData.IngredientName;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
