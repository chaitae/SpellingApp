using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required when using Event data.


public class RemoveWord : MonoBehaviour, IPointerDownHandler
{

    public void OnPointerDown(PointerEventData eventData)
    {
        if(SpellingListManager.Instance)
        SpellingListManager.Instance.RemoveWord(GetComponentInChildren<Text>().text);
        GameObject.Destroy(this);
    }
}
