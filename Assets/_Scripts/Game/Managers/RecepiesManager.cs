using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecepiesManager : MonoBehaviour
{
    public static RecepiesManager Instance { get; private set; }
    public event Action<RecepieDataSO> OnRecepieFound;

    [SerializeField] private List<Ingredient> _ingredientsInMixingZone;
    [SerializeField] private List<RecepieDataSO> _availableRecipes;
    [SerializeField] private List<Image> _recepiesIndexVisuals;

    private Color _openedColor = Color.white;

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
        var openedRecepiesIndexes = PlayerData.Instance.GetOpenedBrainrotsIndexes();

        // Лог для проверки индексов
        Debug.Log($"Найдено открытых рецептов: {openedRecepiesIndexes.Count}");
        foreach (var index in openedRecepiesIndexes)
        {
            Debug.Log($"Индекс для открытия: {index}");
        }

        //Проверка, что кол-ва совпадают
        if (_recepiesIndexVisuals.Count != AvailableRecepies.Count)
        {
            Debug.LogError("Количество картинок рецептов не соответсвует кол-ву доступных рецептов");
            return;
        }

        //Инициализация картинок 
        for (int i = 0; i < _availableRecipes.Count; i++)
        {
            _recepiesIndexVisuals[i].sprite = _availableRecipes[i].RecepieSprite;
        }

        //Открываем уже открытые картинки
        for (int i = 0; i < openedRecepiesIndexes.Count; i++)
        {
            int openedIndex = openedRecepiesIndexes[i];
            _recepiesIndexVisuals[openedIndex].color = _openedColor;
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
                OpenRecepieInIndex(recipe);
                return;
            }
        }
    }

    private void OpenRecepieInIndex(RecepieDataSO recepieData)
    {
        var indexOfFoundRecepie = _availableRecipes.IndexOf(recepieData);
        _recepiesIndexVisuals[indexOfFoundRecepie].color = _openedColor;
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