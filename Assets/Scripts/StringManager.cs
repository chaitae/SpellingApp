using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct WordPair
{
    public string correctSpelling;
    public string enteredSpelling;
}
public class StringManager : MonoBehaviour {
    private static StringManager _instance;
    public GameObject endGameScreen;
    public GameObject inGameScreen;
    public Text endGameScore;
    public Text textGui;
    public Text scoreGui;
    public Text failGui;
    public Text endGameCommentGui;
    public Text youShouldLookAtThis;
    public string enteredString;
    public string currentWord = "default";
    public Transform winObject;
    public Transform letterBlock;
    public GameObject parent;
    public float spacing = 4f;

    private List<string> gameList = new List<string>();
    AudioSource audioSource;
    public AudioClip loseClip;
    public AudioClip superWinClip;
    private IEnumerator coroutine;
    public int tries = 0;
    int completedGameQt;
    int failCounter = 0;
    int points = 0;
    public int correctAnswers = 0;
    private int maxFails = 2;
    private List<string> missedWords = new List<string>();
    public GameObject parentWordBlank;
    public GameObject wordBlank;
    bool isTestMode = false;
    public List<WordPair> answerPairs = new List<WordPair>();
    public List<WordPair> answerPairsCorrect = new List<WordPair>();


    public static StringManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("StringManager");
                go.AddComponent<StringManager>();
            }
            return _instance;
        }
    }
    void Awake() {
        enteredString = "";
        _instance = this;
        EasyTTSUtil.SpeechFlush("Hello");
        audioSource = this.GetComponent<AudioSource>();
        //OpenKeyboard();

    }
    public void SetTestMode(bool isTest)
    {
        isTestMode = isTest;
        StartGame();
    }
    public void StartGame()
    {
        missedWords.Clear();
        failGui.gameObject.SetActive(false);
        textGui.text = "";
        scoreGui.text = "Score:0";
        enteredString = "";
        points = 0;
        tries = 0;
        correctAnswers = 0;
        failCounter = -1;
        SetGamelist();
        answerPairs.Clear();
        SetWord();
        //SayWord();
        EasyTTSUtil.SpeechFlush(StringManager.Instance.currentWord,1,.2f,1);
        EasyTTSUtil.SpeechFlush(StringManager.Instance.currentWord,1,.2f,1);
    }
    public void SetGamelist()
    {
        if (!isTestMode)
        {
            gameList = SpellingListManager.Instance.GetGameList();
            maxFails = 2;
        }
        else
        {
            gameList = SpellingListManager.Instance.GetTestList();
            maxFails = 0;
        }
    } 
    public void SayWord()
    {
        coroutine = WaitAndTalk();
        StartCoroutine(coroutine);
        Debug.Log(StringManager.Instance.currentWord);
    }
    public void ClearWrongText()
    {
        IEnumerator cor = WaitandClear();
        StartCoroutine(cor);

    }
    private IEnumerator WaitandClear()
    {
        yield return new WaitForSeconds(.5f);
        textGui.text = "";
    }
    //public void HideFailText()
    //{
    //    IEnumerator cor = FailTextWait();
    //    StartCoroutine(cor);
    //}
    private IEnumerator WaitAndTalk()
    {
        yield return new WaitForSeconds(.5f);
        EasyTTSUtil.SpeechFlush(StringManager.Instance.currentWord,1,.2f,1);
    }
    private IEnumerator FailTextWait()
    {
        yield return new WaitForSeconds(4f);
        failGui.gameObject.SetActive(false);
        textGui.text = "";
        SayWord();

    }
    public void SetWord()
    {
        if(gameList.Count > 0)
        {
            string randomWord = gameList[Random.Range(0, gameList.Count - 1)];
            currentWord = randomWord;
        }
    }
    public void AddLetter(string letter)
    {

        failGui.gameObject.SetActive(false);
        enteredString += letter;
        textGui.text = enteredString;
    }
    public void RemoveLetter()
    {
        enteredString = enteredString.Substring(0, enteredString.Length - 1);
        textGui.text = enteredString;

    }
    public void SetUpEndScreen()
    {
        Results results = new Results();
        int index = SpellingListManager.Instance.currIndex;
        results.listName = SpellingListManager.Instance.masterList[index].name;
        results.answerPairs = answerPairs;
        results.answerPairsCorrect = answerPairsCorrect;
        results.totalWords = SpellingListManager.Instance.masterList[index].list.Count;
        results.date = System.DateTime.Now + "";

        ResultManager.Instance.resultsList.Add(results);
        //ResultManager.Instance.SelectList(index);
        Debug.Log(ResultManager.Instance.resultsList[0].listName);
        //ResultManager.Instance.resultsList.Add()
        float percentage = correctAnswers / tries;
        if(percentage == 1)
        {
            youShouldLookAtThis.text = "Wow! You got a perfect score!";
        }
        if(percentage > .9f)
        {
            audioSource.PlayOneShot(superWinClip);
            endGameCommentGui.text = "Great Job!";
        }
        else if(percentage >= .8f)
        {
            audioSource.PlayOneShot(superWinClip);
            endGameCommentGui.text = "You're Getting there!";
        }
        else if (percentage >= .7f)
        {
            endGameCommentGui.text = "Keep Going!";
        }
        else if (percentage <= .6f)
        {
            endGameCommentGui.text = "You can do this.";
        }
        endGameScore.text = "You got " + correctAnswers + " out of " + tries + " correct";
        endGameScreen.SetActive(true);
        inGameScreen.SetActive(false);
        completedGameQt++;
        foreach (Transform child in parentWordBlank.GetComponentsInChildren<Transform>())
        {
            if(parentWordBlank != child.gameObject)
            Destroy(child.gameObject);
        }
        foreach (WordPair word in answerPairs)
        {
            //word.correctSpelling
            GameObject temp = Instantiate(wordBlank, parentWordBlank.transform);
            Text[] texts = temp.GetComponentsInChildren<Text>();
            texts[0].text = word.correctSpelling;
            texts[1].text = word.enteredSpelling;
        }
    }
    void WrongWordAction()
    {
        audioSource.PlayOneShot(loseClip);
        textGui.text = "wrong";
        points = points - 10;
        scoreGui.text = "Score: " + points;
        failCounter++;
        ClearWrongText();
    }
    public void EnterString()
    {
        //Debug.Log("hello?");
        tries++;
        gameList.Remove(currentWord);
        Debug.Log(currentWord+" "+gameList.Count);
        textGui.text = "";

        if (isTestMode)
        {
            gameList.Remove(currentWord);
            if(enteredString.CompareTo(currentWord) == 0 )
            {
                failCounter = 0;
                correctAnswers++;
                points = points + 10;
                WordPair wordPair = new WordPair();
                wordPair.correctSpelling = currentWord;
                wordPair.correctSpelling = enteredString;
                answerPairsCorrect.Add(wordPair);
            }
            else
            {
                WordPair temp = new WordPair();
                temp.correctSpelling = currentWord;
                temp.enteredSpelling = enteredString;
                answerPairs.Add(temp);
                WrongWordAction();
            }
            SetWord();
            if(gameList.Count > 0)
            SayWord();
        }
        else
        {
            if (enteredString.CompareTo(currentWord) == 0)
            {
                audioSource.Play();
                textGui.text = "Good Job!";
                failCounter = 0;
                correctAnswers++;
                points = points + 10;
                gameList.Remove(currentWord);
            }
            else
            {
                if(failCounter >= maxFails)
                {
                    failGui.gameObject.SetActive(true);
                    points = points - 10;
                    scoreGui.text = "Score: " + points;
                    failCounter = 0;
                    textGui.text = currentWord;
                    SetWord();
                }
                else
                {
                    Debug.Log(failCounter + " fail counter");
                    failCounter++;
                    WordPair temp = new WordPair();
                    temp.correctSpelling = currentWord;
                    temp.enteredSpelling = enteredString;
                    answerPairs.Add(temp);
                    WrongWordAction();
                }

            }
        }
        if (gameList.Count == 0)
        {
            SetUpEndScreen();
        }
        enteredString = "";
    }
}
