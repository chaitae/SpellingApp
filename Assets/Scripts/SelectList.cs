using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required when using Event data.

public class SelectList : MonoBehaviour, IPointerDownHandler
{
    public int index;
    public SpellingList spellList;
    // Use this for initialization
    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<Button>().Select();
        //spellList = SpellingListManager.Instance.masterList[SpellingListManager.Instance.FindIndexFromName(spellList.name)];
        if (SpellingListManager.Instance)
            SpellingListManager.Instance.SelectList(spellList,gameObject);
    }
}
