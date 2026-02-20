using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recepie", menuName = "Recepies/Recepie Data")]
public class RecepieDataSO : ScriptableObject
{
    public int RecepieID => _recepieId;
    public string RecepieName => _recepieName;
    public List<IngredientDataSO> RequiredIngredients => _ingredients;
    public Sprite RecepieSprite => _recepieSprite;

    [SerializeField] private int _recepieId;
    [SerializeField] private string _recepieName;
    [SerializeField] private Sprite _recepieSprite;
    [SerializeField] private List<IngredientDataSO> _ingredients;
    [SerializeField] private Rarity _rarity;

    enum Rarity
    {
        Common,
        Rare,
        Legendary,
        Mythic,
        BrainrotGod
    }
}
