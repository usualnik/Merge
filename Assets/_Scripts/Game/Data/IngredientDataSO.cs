using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredient", menuName = "Ingredients/Ingredient Data")]
public class IngredientDataSO : ScriptableObject
{
    public string IngredientName => _ingredientName;
    public Sprite IngredientSprite => _ingredientSprite;

    [SerializeField] private string _ingredientName;

    [SerializeField] private Sprite _ingredientSprite;
}
