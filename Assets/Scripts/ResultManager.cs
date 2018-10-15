using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Results
{
    public string date;
    public string listName;
    int correctWords;
    public int totalWords;
    public List<WordPair> answerPairs = new List<WordPair>();
    public List<WordPair> answerPairsCorrect = new List<WordPair>();
}
public class ResultManager : MonoBehaviour
{
    public GameObject listParent;
    public GameObject wordPairParent;
    public Text resultText;
    public Text dateText;

    public GameObject listPrefab;
    public GameObject wordPairPrefab;
    public static ResultManager _instance;
    public List<Results> resultsList = new List<Results>();
    int currIndex = 0;

    public static ResultManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("ResultManager");
                go.AddComponent<ResultManager>();
            }
            return _instance;
        }
    }
    void TestInput()
    {
        Results results = new Results();
        int index = SpellingListManager.Instance.currIndex;
        results.listName = SpellingListManager.Instance.masterList[index].name;
        WordPair answerPairs = new WordPair();
        answerPairs.correctSpelling = "cgews";
        answerPairs.enteredSpelling = "sdag";
        List<WordPair> blah = new List<WordPair>();
        blah.Add(answerPairs);
        results.answerPairs = blah;
        results.answerPairsCorrect = blah;
        results.totalWords = SpellingListManager.Instance.masterList[index].list.Count;
        results.date = System.DateTime.Now + "";
        resultsList.Add(results);


        int index2 = SpellingListManager.Instance.currIndex;
        results.listName = "second";
        answerPairs = new WordPair();
        answerPairs.correctSpelling = "Second";
        answerPairs.enteredSpelling = "entered";
        blah = new List<WordPair>();
        blah.Add(answerPairs);
        results.answerPairs = blah;
        results.answerPairsCorrect = blah;
        results.totalWords = SpellingListManager.Instance.masterList[index].list.Count;
        results.date = System.DateTime.Now + "";
        resultsList.Add(results);
    }
    void Awake()
    {
        TestInput();
        if(resultsList.Count>0)
        {
            //SelectList(resultsList.Count - 1);
        }
        //Results results = new Results();
        //int index = SpellingListManager.Instance.currIndex;
        //results.listName = SpellingListManager.Instance.masterList[index].name;
        //WordPair answerPairs = new WordPair();
        //answerPairs.correctSpelling = "correct";
        //answerPairs.enteredSpelling = "entered";
        //List<WordPair> blah = new List<WordPair>();
        //blah.Add(answerPairs);
        //results.answerPairs = blah;
        //results.answerPairsCorrect = blah;
        //results.totalWords = SpellingListManager.Instance.masterList[index].list.Count;
        //results.date = System.DateTime.Now + "";
        //resultsList.Add(results);
        _instance = this;
        InitializePage();
        Debug.Log("awake");

    }
    public void SelectList(int i)
    {

        currIndex = i;
        if (resultsList.Count > 0)
        {
            resultText.text = "Got " + resultsList[i].answerPairsCorrect.Count + " out of " + resultsList[i].totalWords;
            dateText.text = resultsList[i].date;
            Debug.Log(resultsList[i].listName);
            FillWordPairs();
            //FillResultsList();
        }
    }
    void FillWordPairs()
    {
        foreach (Transform child in wordPairParent.GetComponentInChildren<Transform>())
        {
            if (child != wordPairParent.transform)
            {
                Destroy(child.gameObject);
            }
        }
        foreach (WordPair pairs in resultsList[currIndex].answerPairs)
        {
            Debug.Log("answerPairsResult: "+pairs.correctSpelling);
            GameObject pairButton = Instantiate(wordPairPrefab, wordPairParent.transform);
            pairButton.GetComponentsInChildren<Text>()[0].text = pairs.correctSpelling;
            pairButton.GetComponentsInChildren<Text>()[1].text = pairs.enteredSpelling;
        }
        foreach (WordPair pairsCorrect in resultsList[currIndex].answerPairsCorrect)
        {
            GameObject pairButton = Instantiate(wordPairPrefab, wordPairParent.transform);
            pairButton.GetComponentsInChildren<Text>()[0].text = pairsCorrect.correctSpelling;
            pairButton.GetComponentsInChildren<Text>()[1].text = pairsCorrect.correctSpelling;
            pairButton.GetComponentsInChildren<Text>()[1].color = Color.blue;
        }
    }
    void FillResultsList()
    {
        int resultIndex = 0;
        foreach(Transform child in listParent.GetComponentInChildren<Transform>())
        {
            if(child != listParent)
            {
                Destroy(child.gameObject);
            }
        }
        //for(int i = resultsList.Count-1; i >= 0; i--)
        //{
        //    resultIndex = i;
        //    Results results = resultsList[i];
        //    GameObject listButton = Instantiate(listPrefab, listParent.transform);
        //    listButton.GetComponent<SelectResultsButton>().index = resultIndex;
        //    Debug.Log("resultsname" + results.listName);
        //    listButton.GetComponentInChildren<Text>().text = results.listName;

        //}
        foreach (Results results in resultsList)
        {
            GameObject listButton = Instantiate(listPrefab, listParent.transform);
            listButton.transform.SetAsFirstSibling();
            listButton.GetComponent<SelectResultsButton>().index = resultIndex;
            Debug.Log("resultsname" + results.listName);
            listButton.GetComponentInChildren<Text>().text = results.listName;
            resultIndex++;
        }
    }
    void InitializePage()
    {
        if(resultsList.Count > 0)
        {
            FillResultsList();
            //currIndex = listPrefab.GetComponents<SelectResultsButton>()[listPrefab.GetComponentsInChildren<Transform>().Length].index;
            //SelectList(currIndex);

            FillWordPairs();
        }
    }

}
