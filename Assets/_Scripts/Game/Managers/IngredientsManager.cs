using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IngredientsManager : MonoBehaviour
{
    public static IngredientsManager Instance { get; private set; }
    public List<IngredientDataSO> HighlightedIngredients { get; private set; }

    [Header("Ingredient References")]
    [SerializeField] private List<IngredientDataSO> _ingredientDatas;
    [SerializeField] private List<Ingredient> _ingredients;

    [Header("Flight Settings")]
    [SerializeField] private RectTransform mixingZoneTransform;
    [SerializeField] private float moveDuration = 1f;

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
            Debug.LogError("Количество ингредиентов не равно количеству исходников");
            return;
        }

        for (int i = 0; i < _ingredients.Count; i++)
        {
            _ingredients[i].InitIngredient(_ingredientDatas[i]);
        }
    }

    public void FlyIngredientsToMixingZone(List<IngredientDataSO> ingredientsToFly)
    {
        if (mixingZoneTransform == null)
        {
            Debug.LogError("Mixing Zone Transform is not assigned!");
            return;
        }

        StartCoroutine(FlyIngredientsCoroutine(ingredientsToFly));
    }

    private IEnumerator FlyIngredientsCoroutine(List<IngredientDataSO> ingredientsToFly)
    {
        foreach (var ingredientData in ingredientsToFly)
        {
            Ingredient sourceIngredient = FindIngredientByData(ingredientData);

            if (sourceIngredient != null && sourceIngredient.gameObject.activeInHierarchy)
            {
                GameManager.Instance.IsSomethingGoingOn = true;

                // Создаем копию
                GameObject flyIngredientObj = Instantiate(sourceIngredient.gameObject,
                    sourceIngredient.transform.position,
                    Quaternion.identity,
                    mixingZoneTransform);

                Ingredient flyIngredient = flyIngredientObj.GetComponent<Ingredient>();

                if (flyIngredient.GetComponentInChildren<IngredientCellVisual>() != null)
                {
                   flyIngredient.GetComponentInChildren<IngredientCellVisual>().SetVisualActive(false);
                }

                // Летим к цели
                flyIngredientObj.GetComponent<RectTransform>()
                    .DOAnchorPos(Vector2.zero, moveDuration)
                    .OnComplete(() =>
                    {
                        RecepiesManager.Instance.PutItemInMixingZone(flyIngredient);
                        flyIngredient.DestroySelf(1f);

                        GameManager.Instance.IsSomethingGoingOn = false;

                    });
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

  

    private Ingredient FindIngredientByData(IngredientDataSO data)
    {
        foreach (var ingredient in _ingredients)
        {
            if (ingredient.IngredientData == data)
                return ingredient;
        }
        return null;
    }
}