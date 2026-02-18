using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IngredientsManager : MonoBehaviour
{
    public static IngredientsManager Instance { get; private set; }

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
    }

    private void Start()
    {
        InitIngredients();
    }

    private void InitIngredients()
    {
        if (_ingredients.Count != _ingredientDatas.Count)
        {
            Debug.LogError("Количество ингредиентов не равно количеству исходников");
        }

        for (int i = 0; i < _ingredients.Count; i++)
        {
            _ingredients[i].InitIngredient(_ingredientDatas[i]);
        }
    }

}
