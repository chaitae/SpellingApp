using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CheckIfMisspelled : MonoBehaviour {

    InputField input;
    public GameObject misspell;
    Text misspellText;
    // Use this for initialization
    void Start() {
        input = GetComponent<InputField>();
        input.onValueChanged.AddListener(delegate { ChangeSpellSquiggly(); });
        misspellText = misspell.GetComponent<Text>();
    }
    public void ChangeSpellSquiggly()
    {
        if(!SpellChecker.Instance.IsSpelledRight(input.text))
        {
            misspell.SetActive(true);
            SetNumberSquiggles(input.text.Length);
        }
        else
        {
            misspell.SetActive(false);
        }
    }
    void SetNumberSquiggles(int sizeWord)
    {
        string temp = "";
        for(int i = 0; i< sizeWord; i++)
        {
            temp += "~";
        }
        misspell.GetComponent<Text>().text = temp;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
