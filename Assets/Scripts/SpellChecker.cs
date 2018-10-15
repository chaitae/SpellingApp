using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellChecker : MonoBehaviour {
    static SpellChecker _instance;
    private string _alphabet = "abcdefghijklmnopqrstwuvxyz";
    private Dictionary<string, int> _dictionary = new Dictionary<string, int>();

    private Dictionary<char, Dictionary<char, List<string>>> _wordsTree = new Dictionary<char, Dictionary<char, List<string>>>();
    public TextAsset wordsDocument;
    public GameObject missSquiggles;
    public static SpellChecker Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject("SpellChecker");
                go.AddComponent<SpellChecker>();
                _instance = new SpellChecker();
            }
            return _instance;
        }
    }
    // Use this for initialization
    void Awake()
    {
        _instance = this;
        foreach (string __word in wordsDocument.text.Split('\n'))
        {
            string __lowerCaseWord = __word.ToLower().TrimEnd();
            _dictionary.Add(__lowerCaseWord,1);
        }

        
    }
    public void ShowSquiggly(string word)
    {
        if(IsSpelledRight(word))
        {
            Debug.Log("yep");
        }
        else
        {
            Debug.Log("wrong");
        }
    }
    public bool IsSpelledRight(string word)
    {
        if(_dictionary.ContainsKey(word))
        {
            return true;   
        }
        return false;

    }
}
