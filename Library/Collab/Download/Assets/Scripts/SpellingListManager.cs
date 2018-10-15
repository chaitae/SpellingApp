using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpellingListManager : MonoBehaviour {
    private static SpellingListManager _instance;
    public GameObject parentWord;
    public GameObject parentList;
    public GameObject wordPrefab;
    public GameObject listPrefab;
    public Dictionary<string, int> wordList = new Dictionary<string, int>();
    public List<SpellingList> masterList = new List<SpellingList>();
    public GameObject currentSelected;
    GameData gameData;
    int currIndex = 0;
    public static SpellingListManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("SpellingListManager");
                go.AddComponent<SpellingListManager>();
            }
            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;
        InitializeList();
    }

    public bool IsDuplicateListName(string tempName)
    {
        foreach(SpellingList list in masterList)
        {
            if(tempName == list.name)
            {
                return true;
            }
        }
        return false;
    }

    public void AddList(string listName)
    {
        listName = listName.Trim();
        Debug.Log(listName +"yelp");
        if(listName != "" && !IsDuplicateListName(listName) && listName != "\n")
        {
            GameObject temp = Instantiate(listPrefab, parentList.transform);
            SelectList selectListComp = temp.GetComponent<SelectList>();
            RemoveList removeListComp = temp.GetComponentInChildren<RemoveList>();
            List<string> list = new List<string>();
            SpellingList newSpellList = new SpellingList(list, listName, masterList.Count);
            temp.GetComponentInChildren<Text>().text = listName;
            selectListComp.spellList = newSpellList;
            removeListComp.spellList = newSpellList;
            masterList.Add(newSpellList);
            gameData.UpdateMasterList(masterList);
            SelectList(newSpellList, temp);
            SaveLoadManager.Save(gameData);
        }

    }
    public void PrintCurrList(List<string> list)
    {
        Debug.Log("currentList:");
        foreach(string word in list)
        {
            Debug.Log(word);
        }
    }
    public int FindIndexFromName(string name)
    {
        for(int i = 0; i< masterList.Count; i++)
        {
            if(masterList[i].name == name)
            {
                return i;
            }
        }
        return -1;
    }
    public void SelectList(SpellingList list, GameObject gameObject)
    {
        int index = FindIndexFromName(list.name);
        currIndex = index;
        wordList.Clear();
        foreach (string word in masterList[currIndex].list)
        {
            wordList.Add(word, 0);
        }
        UpdateWords();
    }
    public void UpdateWords()
    {
        Transform[] children =parentWord.GetComponentsInChildren<Transform>();
        foreach(Transform child in children)
        {
            if(child.name != "Content")
            GameObject.Destroy(child.gameObject);
        }
        foreach (String word in ReturnRawList())
        {
            GameObject wordPrefabTemp = Instantiate(wordPrefab, parentWord.transform);
            wordPrefabTemp.GetComponentInChildren<Text>().text = word;
            if (!SpellChecker.Instance.IsSpelledRight(word))
            {
                wordPrefabTemp.transform.Find("Misspell").gameObject.SetActive(true);
                SetNumberSquiggles(word.Length);
            }
        }
    }
    public void RemoveList(SpellingList list)
    {
        Debug.Log(list.name + " " +FindIndexFromName(list.name));
        Transform[] children = parentWord.GetComponentsInChildren<Transform>();

        if (currIndex == FindIndexFromName(list.name) || masterList.Count-1 == 0)
        {
            foreach (Transform child in children)
            {
                if (child.name != "Content")
                    GameObject.Destroy(child.gameObject);
            }
        }
        masterList.RemoveAt(FindIndexFromName(list.name));
        gameData.UpdateMasterList(masterList);

        SaveLoadManager.Save(gameData);

    }
    public List<String> GetTestList()
    {
        List<string> rawList = new List<string>();
        rawList = ReturnRawList();
        int size = rawList.Count;
        return rawList;
    }
    public List<string> GetGameList()
    {
        List<string> rawList = new List<string>();
        rawList = ReturnRawList();
        int size = rawList.Count;
        for (int i = 0; i < size; i++)
        {
            rawList.Add(rawList[i]);
            rawList.Add(rawList[i]);
        }
        return rawList;
    }
    public void InstantiateLists()
    {
        if (masterList.Count != 0)
        {
            InstantiateWords();
        }
        for (int i = 0; i < SaveLoadManager.Load().masterList.Count; i++)
        {
            GameObject temp = Instantiate(listPrefab, parentList.transform);
            temp.GetComponent<SelectList>().index = i;
            temp.GetComponent<SelectList>().spellList = masterList[i];
            temp.GetComponentInChildren<Text>().text = masterList[i].name;
            temp.GetComponentInChildren<RemoveList>().spellList = masterList[i];
        }
    }
    public void InstantiateWords()
    {
        foreach (String word in ReturnRawList())
        {
            GameObject temp = Instantiate(wordPrefab, parentWord.transform);
            temp.GetComponentInChildren<Text>().text = word;
        }

    }
    public void InitializeList()
    {
        gameData = SaveLoadManager.Load();
        LoadList();
        InstantiateLists();
    }
    public void SaveList()
    {
        masterList[currIndex].list = ReturnRawList();
        gameData = new GameData(currIndex,masterList);
        SaveLoadManager.Save(gameData);
    }
    public void LoadList()
    {
        List<string> savedList = SaveLoadManager.Load().currentWordList;
        masterList = SaveLoadManager.Load().masterList;
        wordList.Clear();
        foreach (String word in savedList)
        {
            wordList.Add(word, 0);
        }

    }
    public List<string> ReturnRawList()
    {
        List<string> keyList = new List<string>(wordList.Keys);
        return keyList;
    }
    void SetNumberSquiggles(int sizeWord)
    {
        string temp = "";
        for (int i = 0; i < sizeWord; i++)
        {
            temp += "~";
        }
        if(transform.Find("Misspell"))
        transform.Find("Misspell").GetComponent<Text>().text = temp;
    }
    public void AddWord(string word)
    {
        word.Trim();
        if(masterList.Count != 0 && word != "")
        {
            if(!wordList.ContainsKey(word))
            {
                wordList.Add(word, 0);
                GameObject temp = Instantiate(wordPrefab, parentWord.transform);
                temp.GetComponentInChildren<Text>().text = word;
                if (!SpellChecker.Instance.IsSpelledRight(word))
                {
                    temp.transform.Find("Misspell").gameObject.SetActive(true);
                    SetNumberSquiggles(word.Length);
                }
                else
                {
                    temp.transform.Find("Misspell").gameObject.SetActive(false);

                }
                masterList[currIndex].list.Add(word);
                SaveList();
                LoadList();
            }
        }
        else
        {
            //error message
        }

    }
    public void RemoveWord(string word)
    {
        wordList.Remove(word);
        masterList[currIndex].list.Remove(word); 
        SaveList();
        LoadList();
    }
}
