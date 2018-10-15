using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Required when using Event data.

public class SelfDestruct : MonoBehaviour, IPointerDownHandler
{
    public GameObject parent;
    public void DestroySelf()
    {
        GameObject.Destroy(gameObject);

    }
    void OnMouseDown()
    {
        GameObject.Destroy(gameObject);
        StringManager.Instance.Reformat();
        Debug.Log("ohnmousedown");
        //call string manager
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (parent)
            GameObject.Destroy(parent);
        GameObject.Destroy(gameObject);
        StringManager.Instance.Reformat();
    }
}
