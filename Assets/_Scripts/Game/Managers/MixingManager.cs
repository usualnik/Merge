using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MixingManager : MonoBehaviour
{
    public static MixingManager Instance { get; private set; }
    public event Action<RecepieDataSO> OnRecepieFound;

    [SerializeField] private List<Ingredient> _ingredientsInMixingZone;
    [SerializeField] private List<RecepieDataSO> _availableRecipes;
    [SerializeField] private List<Image> _recepiesVisual;

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
        InitRecepies();
    }

    private void InitRecepies()
    {
        if (_recepiesVisual.Count != AvailableRecepies.Count)
        {
            Debug.LogError("Количество картинок рецептов не соответсвует кол-ву доступных рецептов");
            return;
        }

        for (int i = 0; i < _availableRecipes.Count; i++)
        {
            _recepiesVisual[i].sprite = _availableRecipes[i].RecepieSprite;
        }
    }


    public void PutItemInMixingZone(Ingredient ingredient)
    {
        if (ingredient == null || ingredient.IngredientData == null)
        {
            Debug.LogError("Попытка добавить некорректный ингредиент");
            return;
        }

        if (!_ingredientsInMixingZone.Contains(ingredient))
        {
            _ingredientsInMixingZone.Add(ingredient);
            Debug.Log($"Добавлен ингредиент: {ingredient.IngredientData.IngredientName}. Всего: {_ingredientsInMixingZone.Count}");
            CheckForRecipes();
        }
    }

    private void CheckForRecipes()
    {
        if (_availableRecipes == null || _availableRecipes.Count == 0)
            return;

        foreach (RecepieDataSO recipe in _availableRecipes)
        {
            if (IsRecipeMatched(recipe))
            {
                OnRecepieFound?.Invoke(recipe);
                return;
            }
        }
    }

    private bool IsRecipeMatched(RecepieDataSO recipe)
    {

        List<IngredientDataSO> requiredCopy = new List<IngredientDataSO>(recipe.RequiredIngredients);

        List<Ingredient> currentCopy = new List<Ingredient>(_ingredientsInMixingZone);

        foreach (IngredientDataSO required in requiredCopy)
        {
            bool found = false;

            for (int i = 0; i < currentCopy.Count; i++)
            {
                if (currentCopy[i].IngredientData == required)
                {
                    found = true;
                    currentCopy.RemoveAt(i);
                    break;
                }
            }

            if (!found)
                return false;
        }

        return true;
    }

    public void ClearMixingSpace()
    {
        foreach (var item in _ingredientsInMixingZone)
        {
            if (item != null)
                item.DestroySelf();
        }

        _ingredientsInMixingZone.Clear();
        Debug.Log("Зона смешивания очищена");
    }

    public List<RecepieDataSO> AvailableRecepies => _availableRecipes;
}