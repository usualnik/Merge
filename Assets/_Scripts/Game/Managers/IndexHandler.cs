using UnityEngine;

public class IndexHandler : MonoBehaviour
{
    [SerializeField] private GameObject _indexPanel;


    public void TogglePanel()
    {
        _indexPanel.SetActive(!_indexPanel.activeInHierarchy);
    }
}
