using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SayWord : MonoBehaviour, IPointerDownHandler
{
    public float volume = 1;
    public float rate = .2f;
    public float pitch = 1f;
    public void OnPointerDown(PointerEventData eventData)
    {
        EasyTTSUtil.SpeechFlush(StringManager.Instance.currentWord, volume, rate, pitch);
    }

}
