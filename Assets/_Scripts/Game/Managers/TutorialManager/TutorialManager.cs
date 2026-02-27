using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [SerializeField] private GameObject _tutorialCanvas;

    [SerializeField] private TutorialElement _firstElement;
    [SerializeField] private GameObject _firstElementPointer;
    [SerializeField] private TutorialElement _secondElement;
    [SerializeField] private GameObject _secondElementPointer;

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
        if (PlayerData.Instance.GetIsFirstTimePlaying())
        {
            _tutorialCanvas.SetActive(true);
            _firstElement.OnTutorialElementClicked += FirstElement_OnTutorialElementClicked;
            _secondElement.OnTutorialElementClicked += SecondElement_OnTutorialElementClicked;
            _firstElementPointer.SetActive(true);
        }else
        {
            _tutorialCanvas.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        _firstElement.OnTutorialElementClicked -= FirstElement_OnTutorialElementClicked;
        _secondElement.OnTutorialElementClicked -= SecondElement_OnTutorialElementClicked;
    }

    private void FirstElement_OnTutorialElementClicked()
    {
        _firstElementPointer.SetActive(false);

        Ingredient ingredient = _firstElement.GetComponent<Ingredient>();
        if (ingredient != null)
        {
            ingredient.StopPulsing();
        }

        _secondElement.gameObject.SetActive(true);
        _secondElementPointer.SetActive(true);
    }

    private void SecondElement_OnTutorialElementClicked()
    {
        _tutorialCanvas.SetActive(false);

        PlayerData.Instance.ChangeFisrtTimePlaying(false);

        Ingredient ingredient = _secondElement.GetComponent<Ingredient>();
        if (ingredient != null)
        {
            ingredient.StopPulsing();
        }
    }
}