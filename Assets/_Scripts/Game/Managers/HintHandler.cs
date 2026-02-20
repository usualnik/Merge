using System;
using UnityEngine;
using UnityEngine.UI;

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
        var allRecepies = MixingManager.Instance.AvailableRecepies;
        RecepieDataSO notYetFoundedRecepie = null;

        foreach (var receipt in allRecepies)
        {
            if(!GameManager.Instance.RecepieFoundIndexes.Contains(receipt.RecepieID))
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

        IngredientsManager.Instance.HighlightIngredients(notYetFoundedRecepie.RequiredIngredients);

        OnHintGiven?.Invoke();
    }

}
