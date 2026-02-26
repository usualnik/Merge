using UnityEngine;
using UnityEngine.UI;

public class IngredientCellVisual : MonoBehaviour
{

    private void Awake()
    {
    }

    public void SetVisualActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
