using UnityEngine;
using UnityEngine.UI;

public class IngredientVisual : MonoBehaviour
{
    public Image Visual { get; set; }

    private void Awake()
    {
        Visual = GetComponent<Image>();
    }
}
