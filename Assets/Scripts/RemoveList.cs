using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required when using Event data.

public class RemoveList : MonoBehaviour, IPointerDownHandler{

    public SpellingList spellList;
    public int index;
    // Use this for initialization
    public void OnPointerDown(PointerEventData eventData)
    {
        if (SpellingListManager.Instance)
            SpellingListManager.Instance.RemoveList(spellList);

    }
}
