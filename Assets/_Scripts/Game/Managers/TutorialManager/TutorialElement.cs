using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialElement : MonoBehaviour, IPointerClickHandler
{
    private bool IsClickable = true;
    public event Action OnTutorialElementClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(IsClickable)
        {
            OnTutorialElementClicked?.Invoke();
        }
    } 

    
}
