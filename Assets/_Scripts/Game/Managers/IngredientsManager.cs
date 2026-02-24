using System.Collections.Generic;
using UnityEngine;

public class IngredientsManager : MonoBehaviour
{
    public static IngredientsManager Instance { get; private set; }
    public List<IngredientDataSO> HighlightedIngredients { get; private set; }

    [SerializeField] private List<IngredientDataSO> _ingredientDatas;
    [SerializeField] private List<Ingredient> _ingredients;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameObject.transform.SetParent(null, false);
        }
        else
            Destroy(gameObject);

        HighlightedIngredients = new List<IngredientDataSO>();
    }

    private void Start()
    {
        InitIngredients();
    }

    private void InitIngredients()
    {
        if (_ingredients.Count != _ingredientDatas.Count)
        {
            Debug.LogError(" оличество ингредиентов не равно количеству исходников");
            return;
        }

        for (int i = 0; i < _ingredients.Count; i++)
        {
            _ingredients[i].InitIngredient(_ingredientDatas[i]);
        }
    }


    public void HighlightIngredients(List<IngredientDataSO> ingredientDatasToHighlight)
    {
        if(HighlightedIngredients != null)
            HighlightedIngredients.AddRange(ingredientDatasToHighlight);

        foreach (var ingredient in _ingredients)
        {
            if (ingredientDatasToHighlight.Contains(ingredient.IngredientData))
            {               
                ingredient.StartPulsing();
                Debug.Log($"ѕодсвечиваем: {ingredient.IngredientData.IngredientName}");
            }
            else
            {
               
                ingredient.StopPulsing();
            }
        }
    }


}