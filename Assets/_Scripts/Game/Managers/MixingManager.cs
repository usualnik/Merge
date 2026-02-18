using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MixingManager : MonoBehaviour
{
    public static MixingManager Instance { get; private set; }

    [SerializeField] private List<Ingredient> _ingredientsInMixingZone;
    [SerializeField] private List<RecepieDataSO> _availableRecipes;

    [SerializeField] private GameObject _recepieFoundPanel;
    [SerializeField] private TextMeshProUGUI _recepieFoundText;

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
                _recepieFoundPanel.SetActive(true);
                _recepieFoundText.text = recipe.RecepieName;
                Debug.Log($"Рецепт найден: {recipe.RecepieName}");
                return; 
            }
        }

        // Если рецепт не найден, можно скрыть панель
        // _recepieFoundPanel.SetActive(false);
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

        _recepieFoundPanel.SetActive(false);
        _ingredientsInMixingZone.Clear();
        Debug.Log("Зона смешивания очищена");
    }
}