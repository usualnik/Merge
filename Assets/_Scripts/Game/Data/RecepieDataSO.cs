using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recepie", menuName = "Recepies/Recepie Data")]
public class RecepieDataSO : ScriptableObject
{

    public string RecepieName => _recepieName;
    public List<IngredientDataSO> RequiredIngredients => _ingredients;

    [SerializeField] private string _recepieName;
    [SerializeField] private Sprite _recepieSprite;
    [SerializeField] private List<IngredientDataSO> _ingredients;

    enum Rarity
    {
        Common,
        Rare,
        Legendary,
        Mythic,
        BrainrotGod
    }
}
