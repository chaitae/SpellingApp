using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Required when using Event data.

public class SelectResultsButton : MonoBehaviour, IPointerDownHandler
{
    public int index;
    // Use this for initialization
    public void OnPointerDown(PointerEventData eventData)
    {
        //GetComponent<Button>().Select();
        //spellList = SpellingListManager.Instance.masterList[SpellingListManager.Instance.FindIndexFromName(spellList.name)];
        if (ResultManager.Instance)
            ResultManager.Instance.SelectList(index);
    }
}
