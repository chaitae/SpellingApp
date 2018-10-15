using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public int tries;
    int completedGameQt;
    int failCounter = 0;
    int points = 0;
    public int correctAnswers = 0;
    private int maxFails = 2;
    private List<string> missedWords = new List<string>();
    public GameObject parentWordBlank;
    public GameObject wordBlank;
    bool isTestMode = false;
    // Use this for initialization


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
            Debug.Log(gameList.Count);

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
    public void HideFailText()
    {
        IEnumerator cor = FailTextWait();
        StartCoroutine(cor);
    }
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
        audioSource.PlayOneShot(superWinClip);
        float percentage = correctAnswers / tries;
        Debug.Log(percentage);
        if(percentage == 1)
        {
            youShouldLookAtThis.text = "Wow! You got a perfect score!";
        }
        if(percentage > .9f)
        {
            Debug.Log("wtf");
            endGameCommentGui.text = "Great Job!";
        }
        else if(percentage >= .8f)
        {
            endGameCommentGui.text = "You're Getting there!";
        }
        else if (percentage >= .7f)
        {
            endGameCommentGui.text = "Keep Going!";
        }
        else if (percentage <= .6f)
        {
            endGameCommentGui.text = "You can do this.C:";
        }
        endGameScore.text = "You got " + correctAnswers + " out of " + tries + " correct";
        endGameScreen.SetActive(true);
        inGameScreen.SetActive(false);
        completedGameQt++;
        foreach (string word in missedWords)
        {
            Debug.Log(word);
            Instantiate(wordBlank, parentWordBlank.transform);
            wordBlank.GetComponentInChildren<Text>().text = word;
        }
    }
    public void EnterString()
    {
        if (gameList.Count == 0)
        {
            SetUpEndScreen();

        }
        else
        {
            tries++;
        }
        Debug.Log(currentWord+ " " + enteredString);
        //Debug.Log(System.DateTime.Today.Date);
        if (isTestMode)
        {
            gameList.Remove(currentWord);
        }

        if (enteredString.CompareTo(currentWord) == 0)
        {
            failCounter = 0;
            correctAnswers++;
            if(!isTestMode)
            gameList.Remove(currentWord);
            points = points + 10;

            Debug.Log("spelled it right!");
            if(gameList.Count == 0)
            {
                SetUpEndScreen();
            }
            else
            {
                audioSource.Play();
                SetWord();
                textGui.text = "Good Job!";
                scoreGui.text = "Score: " + points;
                SayWord();
            }

        }
        else if (failCounter == maxFails && !isTestMode)
        {
            failGui.gameObject.SetActive(true);
            points = points - 10;
            scoreGui.text = "Score: " + points;
            failCounter = 0;
            textGui.text = currentWord;
            HideFailText();
            SetWord();
        }
        else
        {
            if(!missedWords.Contains(currentWord))
            missedWords.Add(currentWord);

            audioSource.PlayOneShot(loseClip);
            textGui.text = "wrong";
            points = points - 10;
            scoreGui.text = "Score: " + points;
            failCounter++;
            ClearWrongText();
            if(gameList.Count == 0)
            {
                SetUpEndScreen();
            }
            SayWord();
        }
        enteredString = "";
    }
    public void Reformat()
    {

    }
}
