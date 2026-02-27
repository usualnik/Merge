using System;
using System.Collections;
using UnityEngine;

public class HintHandler : MonoBehaviour
{
    public static HintHandler Instance { get; private set; }
    public event Action OnHintGiven;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null, false);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void GiveHint()
    {
        if (GameManager.Instance.Hints <= 0) return;

        var allRecepies = RecepiesManager.Instance.AvailableRecepies;
        RecepieDataSO notYetFoundedRecepie = null;

        foreach (var receipt in allRecepies)
        {
            if (!GameManager.Instance.RecepieFoundIndexes.Contains(receipt.RecepieID) &&
                !IngredientsManager.Instance.HighlightedIngredients.Contains(receipt.RequiredIngredients[0]))
            {
                notYetFoundedRecepie = receipt;
                break;
            }
        }

        if (notYetFoundedRecepie == null)
        {
            Debug.Log("All recepies are found");
            return;
        }

        //IngredientsManager.Instance.HighlightIngredients(notYetFoundedRecepie.RequiredIngredients);
        IngredientsManager.Instance.FlyIngredientsToMixingZone(notYetFoundedRecepie.RequiredIngredients);

        GameManager.Instance.GiveHint();
        OnHintGiven?.Invoke();
    }
}